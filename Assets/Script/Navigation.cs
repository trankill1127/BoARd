using Google.XR.ARCoreExtensions.GeospatialCreator.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public string spot;
    public double distance;

    public Node(string spot, double distance)
    {
        this.spot = spot;
        this.distance = distance;
    }
}

public class priorityQueue
{

    public List<Node> values;

    public priorityQueue()
    {
        values = new List<Node>();
    }

    public void enqueue(string s, double d)
    {
        Node newNode = new Node(s, d);
        values.Add(newNode);
        bubbleUp();
    }

    public void bubbleUp()
    {
        int idx = values.Count - 1;
        Node node = values[idx];

        while (idx > 0)
        {
            int parentIdx;
            if ((idx % 2) > 0) parentIdx = idx / 2;
            else parentIdx = idx / 2 - 1;
            Node parentNode = values[parentIdx];

            if (node.distance >= parentNode.distance) break;

            values[parentIdx] = node;
            values[idx] = parentNode;
            idx = parentIdx;
        }
    }

    public Node dequeue()
    {
        Node firstNode = values[0];
        Node lastNode = values[values.Count - 1];
        values.RemoveAt(values.Count - 1);

        if (values.Count != 0)
        {
            values[0] = lastNode;
            sinkDown();
        }

        return firstNode;
    }

    public void sinkDown()
    {
        int idx = 0;
        int maxLength = values.Count;
        Node node = values[0];

        Node leftChild, rightChild;
        int swap;

        while (true)
        {
            swap = idx;
            int leftChildIdx = 2 * idx + 1;
            int rightChildIdx = 2 * idx + 2;

            if (leftChildIdx < maxLength)
            {
                leftChild = values[leftChildIdx];
                if (leftChild.distance < node.distance)
                {
                    swap = leftChildIdx;
                }
            }

            if (rightChildIdx < maxLength)
            {
                rightChild = values[rightChildIdx];
                if ((swap == idx && rightChild.distance < node.distance)
                    || (swap != idx && rightChild.distance < values[swap].distance))
                {
                    swap = rightChildIdx;
                }
            }

            if (swap == idx)
            {
                break;
            }

            values[idx] = values[swap];
            values[swap] = node;
            idx = swap;
        }
    }
}

public class weightedGraph
{
    Dictionary<string, List<Node>> adjacentList;

    public weightedGraph()
    {
        adjacentList = new Dictionary<string, List<Node>>();
    }

    public void addVertex(string v)
    {
        if (!adjacentList.ContainsKey(v))
        {
            adjacentList.Add(v, new List<Node>());
        }
    }

    public void addEdge(string v1, string v2, double d)
    {
        adjacentList[v1].Add(new Node(v2, d));
        adjacentList[v2].Add(new Node(v1, d));
    }

    public List<string> dijkstra(string from, string to)
    {
        priorityQueue nearSpots = new priorityQueue();
        Dictionary<string, double> distances = new Dictionary<string, double>();
        Dictionary<string, string> previous = new Dictionary<string, string>();
        List<string> path = new List<string>();
        string smallest = "";

        foreach (string spot in adjacentList.Keys)
        {
            if (spot == from) distances.Add(spot, 0);
            else distances.Add(spot, 30000);
            nearSpots.enqueue(spot, distances[spot]);
            previous.Add(spot, "none");
        }


        while (nearSpots.values.Count > 0)
        {
            //현재 지점에서 가장 가까운 지점
            smallest = nearSpots.dequeue().spot;

            if (smallest == to) //도착지 도착
            {
                while (previous[smallest] != "none")
                {
                    path.Add(smallest);
                    smallest = previous[smallest];
                }
                break;
            }

            //도착지 외 지점
            if (smallest != "none" || distances[smallest] != 30000)
            {
                //smallest의 모든 이웃 지점을 체크
                for (int neighborIdx = 0; neighborIdx < adjacentList[smallest].Count; neighborIdx++)
                {
                    Node nextNode = adjacentList[smallest][neighborIdx];
                    double candidateDistance = distances[smallest] + nextNode.distance;
                    string candidateSpot = nextNode.spot;
                    if (candidateDistance < distances[candidateSpot])
                    {
                        distances[candidateSpot] = candidateDistance;
                        previous[candidateSpot] = smallest;
                        nearSpots.enqueue(candidateSpot, candidateDistance);
                    }
                }
            }
        }

        path.Add(smallest); //from 추가
        return path;
    }

}

public class Navigation : MonoBehaviour
{
    void CreateGeospatialAnchor(double latitude, double longitude, string gameObjectName)
    {
        // 새로운 GameObject 생성
        GameObject gameObject = new GameObject();

        // ARGeospatialCreatorAnchor 클래스의 인스턴스 생성
        ARGeospatialCreatorAnchor geospatialAnchor = gameObject.AddComponent<ARGeospatialCreatorAnchor>();

        // Latitude, Longitude, AltType 변수 설정
        geospatialAnchor.Latitude = latitude;   // 위도
        geospatialAnchor.Longitude = longitude; // 경도
        geospatialAnchor.AltType = ARGeospatialCreatorAnchor.AltitudeType.Terrain; // AltitudeType을 Terrain으로 변경

        // Fir_Tree.prefab 로드
        // 프리팹 로드
        GameObject palmTreePrefab = Resources.Load<GameObject>("Darth_Artisan/Free_Trees/Prefabs/Palm_Tree");
        if (palmTreePrefab == null)
        {
            Debug.LogError("프리팹을 로드하는데 실패했습니다.");
            return; // 로드 실패 시 함수 종료
        }

        // palmTreePrefab을 gameObject의 자식으로 추가
        GameObject instantiatedTree = Instantiate(palmTreePrefab, gameObject.transform);
        Debug.Log("Instantiated Tree: " + instantiatedTree);

        // palmTreePrefab 의 크기조정
        instantiatedTree.transform.localScale = new Vector3(10f, 10f, 10f);
        // GameObject 이름 설정
        gameObject.name = gameObjectName;
    }
    void VertexComponentModify(double latitude, double longitude, string vertexName)
    {
        GameObject vertex = GameObject.Find(vertexName);
        Debug.Log(vertexName);
        if (vertex == null)
        {
            Debug.Log("Vertex null");
        }
        ARGeospatialCreatorAnchor arVertex = vertex.GetComponent<ARGeospatialCreatorAnchor>();
        arVertex.Latitude = latitude;
        arVertex.Longitude = longitude;
        arVertex.AltType = ARGeospatialCreatorAnchor.AltitudeType.Terrain;
        arVertex.enabled = true;
        //GameObject UI = GameObject.Find("UI");
        //Destroy(UI);
    }

    public class MapSpot
    {
        public string name;
        public float lat;
        public float lon;
        public float alt;
    }

    static weightedGraph graph = new weightedGraph();
    public string to;

    // Start is called before the first frame update
    void Start()
    {
        setGraph();
        List<string> resultPath = graph.dijkstra("정문", to);
        for (int i = 0; i < resultPath.Count; i++)
        {
            Debug.Log($"'{resultPath[i]}'에 해당하는 좌표: {FindLocationByString(locationDataList, resultPath[i]).Latitude}, {FindLocationByString(locationDataList, resultPath[i]).Longitude}");
            //Debug.Log(FindLocationByString(locationDataList,resultPath[i]).Latitude);
            CreateGeospatialAnchor(FindLocationByString(locationDataList, resultPath[i]).Latitude, FindLocationByString(locationDataList, resultPath[i]).Longitude, $"Vertex ({i})");
            //VertexComponentModify(FindLocationByString(locationDataList, resultPath[i]).Latitude, FindLocationByString(locationDataList, resultPath[i]).Longitude, $"Vertex ({i})");

        }
    }
    class LocationData
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public LocationData(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
    // 문자열에 해당하는 좌표를 찾는 함수
    static LocationData FindLocationByString(List<LocationData> dataList, string target)
    {
        foreach (var data in dataList)
        {
            if (data.Name == target)
            {
                return data;
            }
        }
        return null; // 찾지 못한 경우
    }

    List<LocationData> locationDataList = new List<LocationData>
    {
        new LocationData("후문", 37.552929, 127.072469),
        new LocationData("2", 37.552120, 127.072817),
        new LocationData("3", 37.552757, 127.073613),
        new LocationData("4", 37.552642, 127.073788),
        new LocationData("5", 37.552436, 127.073552),
        new LocationData("6", 37.551919, 127.073466),
        new LocationData("7", 37.552182, 127.073493),
        new LocationData("8", 37.552223, 127.073844),
        new LocationData("9", 37.552070, 127.073654),
        new LocationData("10", 37.551944, 127.073823),
        new LocationData("11", 37.551887, 127.073128),
        new LocationData("12", 37.552106, 127.073995),
        new LocationData("13", 37.551784, 127.073648),
        new LocationData("14", 37.551629, 127.073450),
        new LocationData("15", 37.551576, 127.073910),
        new LocationData("16", 37.551439, 127.073688),
        new LocationData("17", 37.551439, 127.074092),
        new LocationData("18", 37.551271, 127.073904),
        new LocationData("19", 37.551205, 127.073987),
        new LocationData("20", 37.551114, 127.074102),
        new LocationData("21", 37.551280, 127.074302),
        new LocationData("22", 37.551400, 127.074457),
        new LocationData("23", 37.551605, 127.074698),
        new LocationData("24", 37.552022, 127.0746467),
        new LocationData("25", 37.551152, 127.075312),
        new LocationData("26", 37.550896, 127.074987),
        new LocationData("27", 37.550605, 127.074601),
        new LocationData("28", 37.550845, 127.073532),
        new LocationData("29", 37.551068, 127.073349),
        new LocationData("30", 37.551370, 127.073768),
        new LocationData("31", 37.550305, 127.072844),
        new LocationData("32", 37.549780, 127.073428),
        new LocationData("33", 37.549319, 127.073471),
        new LocationData("34", 37.550303, 127.073309),
        new LocationData("35", 37.550464, 127.073514),
        new LocationData("36", 37.550680, 127.073725),
        new LocationData("37", 37.550271, 127.074207),
        new LocationData("38", 37.550080, 127.073585),
        new LocationData("39", 37.550219, 127.073791),
        new LocationData("40", 37.550042, 127.074007),
        new LocationData("41", 37.549965, 127.073702),
        new LocationData("42", 37.550017, 127.073814),
        new LocationData("43", 37.549796, 127.074073),
        new LocationData("44", 37.549881, 127.074177),
        new LocationData("45", 37.549630, 127.074110),
        new LocationData("46", 37.549619, 127.073987),
        new LocationData("47", 37.549707, 127.074334),
        new LocationData("48", 37.549770, 127.074299),
        new LocationData("49", 37.550154, 127.074114),
        new LocationData("50", 37.54985263704142, 127.0744738147587),
        new LocationData("51", 37.549298, 127.074113),
        new LocationData("쪽문1", 37.549302, 127.073370),
        new LocationData("53", 37.548783, 127.073330),
        new LocationData("54", 37.548481, 127.073341),
        new LocationData("55", 37.548764, 127.073716),
        new LocationData("56", 37.548455, 127.073703),
        new LocationData("57", 37.548457, 127.074039),
        new LocationData("58", 37.548416, 127.074027),
        new LocationData("59", 37.548760, 127.074068),
        new LocationData("쪽문2", 37.548348, 127.073351),
        new LocationData("61", 37.548936, 127.074083),
        new LocationData("62", 37.549315, 127.074406),
        new LocationData("63", 37.549853, 127.074876),
        new LocationData("64", 37.54977576945796, 127.07492073907261),
        new LocationData("65", 37.549695, 127.074672),
        new LocationData("66", 37.549661, 127.074639),
        new LocationData("67", 37.54981623350918, 127.07504808999593),
        new LocationData("68", 37.549571, 127.075005),
        new LocationData("69", 37.549535, 127.074966),
        new LocationData("70", 37.549193, 127.074773),
        new LocationData("71", 37.548886, 127.074644),
        new LocationData("정문", 37.549117918851856, 127.07511245901617),
        new LocationData("73", 37.55028894791896, 127.07553376118724),
        new LocationData("쪽문3", 37.55020093930908, 127.07578546538973),
        new LocationData("75", 37.55038784363281, 127.07586769975154),
        new LocationData("76", 37.55050283158843, 127.07569523837321),
        new LocationData("77", 37.551071, 127.076302),
        new LocationData("78", 37.551202, 127.075918),
        new LocationData("79", 37.55100289974998, 127.07567311077963),
        new LocationData("80", 37.5507916963534, 127.07483971041053),
        new LocationData("81", 37.55073534364052, 127.07490331005661),
        new LocationData("82", 37.55103694754837, 127.07526574521138),
        new LocationData("83", 37.551027, 127.075285),
        new LocationData("84", 37.55048734443272, 127.07525670510664),
        new LocationData("86", 37.548890, 127.075007),
        new LocationData("87", 37.552038, 127.073313),
        new LocationData("88", 37.550889, 127.073574),
        new LocationData("89", 37.552378, 127.074155),
        new LocationData("90", 37.54920815341758, 127.07490036725218),
        new LocationData("91", 37.5500237390933, 127.07461261228808),
        new LocationData("92", 37.55138398553519, 127.0750241993248),
        new LocationData("93", 37.548769849748304, 127.07341466117833),
        new LocationData("94", 37.55234279882951, 127.07262032683926),
        new LocationData("대양AI센터", 37.55067866466778, 127.07547898598591),
        new LocationData("대양AI센터1", 37.550566128501785, 127.07534024421014),
        new LocationData("대양AI센터2", 37.55049833294774, 127.07568533181187),
        new LocationData("대양AI센터3", 37.55077093184624, 127.07561204938861),
        new LocationData("대양AI센터4", 37.550845479255656, 127.07527545473171),
        new LocationData("모차르트홀", 37.54837299187981, 127.07408476058303),
        new LocationData("대양홀", 37.54888404833184, 127.07449831283674),
        new LocationData("집현관", 37.54908958952705, 127.07359886582155),
        new LocationData("집현관1", 37.5492742958514, 127.07359621833467),
        new LocationData("집현관2", 37.549010520629885, 127.07396939887298),
        new LocationData("집현관3", 37.54881482570873, 127.07353352676115),
        new LocationData("학생회관", 37.549602156553725, 127.07518933097482),
        new LocationData("학생회관1", 37.549566222813525, 127.07502237732841),
        new LocationData("학생회관2", 37.54930269713431, 127.07499665129299),
        new LocationData("세종관", 37.549969698891054, 127.07458143803036),
        new LocationData("세종관1", 37.54991567832413, 127.07451914347061),
        new LocationData("세종관2", 37.550023737305565, 127.07461544141462),
        new LocationData("군자관", 37.54959176160512, 127.07381437210093),
        new LocationData("군자관2", 37.54959614262905, 127.07401241427353),
        new LocationData("군자관1", 37.54981256968448, 127.07371274151824),
        new LocationData("광개토관", 37.55019129501604, 127.07322084505508),
        new LocationData("광개토관1", 37.55058758638846, 127.07345888129747),
        new LocationData("광개토관2", 37.55025430516801, 127.07331709735423),
        new LocationData("이당관", 37.55035822104793, 127.07283058689191),
        new LocationData("진관홀", 37.55095022208627, 127.0734846996733),
        new LocationData("용덕관", 37.55134223694715, 127.07335494235691),
        new LocationData("용덕관1", 37.55119799993971, 127.07347928459517),
        new LocationData("애지헌", 37.550760781356026, 127.07385513314536),
        new LocationData("영실관", 37.55232431714493, 127.07336722174925),
        new LocationData("충무관", 37.552276587373726, 127.07405184462753),
        new LocationData("충무관1", 37.55219780330275, 127.07396689036213),
        new LocationData("충무관2", 37.55235987281623, 127.07414246196399),
        new LocationData("다산관", 37.55255580952808, 127.07419075287422),
        new LocationData("다산관1", 37.552271765478096, 127.07455543982492),
        new LocationData("다산관2", 37.55247248682488, 127.07415954884834),
        new LocationData("다산관3", 37.55275878921066, 127.07378354471737),
        new LocationData("율곡관", 37.551943267989934, 127.0739722972141),
        new LocationData("우정당", 37.55186847615874, 127.0747021563938),
        new LocationData("우정당1", 37.55195189353698, 127.07458341320121),
        new LocationData("우정당2", 37.55178280795931, 127.0748180678768),
        new LocationData("학술정보원", 37.551526405765, 127.0742123659530),
        new LocationData("박물관", 37.551469484927225, 127.0751742317245)
    };

    static void setGraph()
    {
        //정점 추가
        graph.addVertex("후문"); graph.addVertex("2"); graph.addVertex("3"); graph.addVertex("4"); graph.addVertex("5");
        graph.addVertex("6"); graph.addVertex("7"); graph.addVertex("8"); graph.addVertex("9"); graph.addVertex("10");
        graph.addVertex("11"); graph.addVertex("12"); graph.addVertex("13"); graph.addVertex("14"); graph.addVertex("15");
        graph.addVertex("16"); graph.addVertex("17"); graph.addVertex("18"); graph.addVertex("19"); graph.addVertex("20");
        graph.addVertex("21"); graph.addVertex("22"); graph.addVertex("23"); graph.addVertex("24"); graph.addVertex("25");
        graph.addVertex("26"); graph.addVertex("27"); graph.addVertex("28"); graph.addVertex("29"); graph.addVertex("30");
        graph.addVertex("31"); graph.addVertex("32"); graph.addVertex("33"); graph.addVertex("34"); graph.addVertex("35");
        graph.addVertex("36"); graph.addVertex("37"); graph.addVertex("38"); graph.addVertex("39"); graph.addVertex("40");
        graph.addVertex("41"); graph.addVertex("42"); graph.addVertex("43"); graph.addVertex("44"); graph.addVertex("45");
        graph.addVertex("46"); graph.addVertex("47"); graph.addVertex("48"); graph.addVertex("49"); graph.addVertex("50");
        graph.addVertex("51"); graph.addVertex("쪽문1"); graph.addVertex("53"); graph.addVertex("54"); graph.addVertex("55");
        graph.addVertex("56"); graph.addVertex("57"); graph.addVertex("58"); graph.addVertex("59"); graph.addVertex("쪽문2");
        graph.addVertex("61"); graph.addVertex("62"); graph.addVertex("63"); graph.addVertex("64"); graph.addVertex("65");
        graph.addVertex("66"); graph.addVertex("67"); graph.addVertex("68"); graph.addVertex("69"); graph.addVertex("70");
        graph.addVertex("71"); graph.addVertex("정문"); graph.addVertex("73"); graph.addVertex("쪽문3"); graph.addVertex("75");
        graph.addVertex("76"); graph.addVertex("77"); graph.addVertex("78"); graph.addVertex("79"); graph.addVertex("80");
        graph.addVertex("81"); graph.addVertex("82"); graph.addVertex("83"); graph.addVertex("84"); graph.addVertex("86");
        graph.addVertex("87"); graph.addVertex("88"); graph.addVertex("89"); graph.addVertex("90"); graph.addVertex("91");
        graph.addVertex("92"); graph.addVertex("93"); graph.addVertex("94"); graph.addVertex("대양AI센터"); graph.addVertex("대양AI센터1");
        graph.addVertex("대양AI센터2"); graph.addVertex("대양AI센터3"); graph.addVertex("대양AI센터4"); graph.addVertex("모차르트홀"); graph.addVertex("집현관");
        graph.addVertex("집현관1"); graph.addVertex("집현관2"); graph.addVertex("집현관3"); graph.addVertex("학생회관1"); graph.addVertex("학생회관2");
        graph.addVertex("학생회관"); graph.addVertex("세종관"); graph.addVertex("세종관1"); graph.addVertex("세종관2"); graph.addVertex("군자관");
        graph.addVertex("군자관1"); graph.addVertex("군자관2"); graph.addVertex("광개토관"); graph.addVertex("광개토관1"); graph.addVertex("광개토관2");
        graph.addVertex("이당관"); graph.addVertex("진관홀"); graph.addVertex("용덕관"); graph.addVertex("용덕관1"); graph.addVertex("애지헌");
        graph.addVertex("영실관"); graph.addVertex("충무관"); graph.addVertex("충무관1"); graph.addVertex("충무관2"); graph.addVertex("다산관");
        graph.addVertex("다산관1"); graph.addVertex("다산관2"); graph.addVertex("다산관3"); graph.addVertex("율곡관"); graph.addVertex("우정당");
        graph.addVertex("우정당1"); graph.addVertex("우정당2"); graph.addVertex("학술정보원"); graph.addVertex("박물관"); graph.addVertex("대양홀");

        //간선 추가
        graph.addEdge("대양AI센터", "대양AI센터1", 24);
        graph.addEdge("대양AI센터", "대양AI센터2", 27);
        graph.addEdge("대양AI센터", "대양AI센터3", 32);
        graph.addEdge("대양AI센터", "대양AI센터4", 38);
        graph.addEdge("집현관", "집현관1", 22);
        graph.addEdge("집현관", "집현관2", 30);
        graph.addEdge("집현관", "집현관3", 26);
        graph.addEdge("학생회관", "학생회관1", 10);
        graph.addEdge("68", "학생회관1", 12);
        graph.addEdge("학생회관", "학생회관2", 23);
        graph.addEdge("세종관", "세종관1", 7);
        graph.addEdge("세종관", "세종관2", 8);
        graph.addEdge("군자관", "군자관1", 25);
        graph.addEdge("군자관", "군자관2", 15);
        graph.addEdge("광개토관", "광개토관1", 37);
        graph.addEdge("광개토관", "광개토관2", 7);
        graph.addEdge("용덕관", "용덕관1", 16);
        graph.addEdge("충무관", "충무관1", 15);
        graph.addEdge("충무관", "충무관2", 9);
        graph.addEdge("다산관", "다산관1", 44);
        graph.addEdge("다산관", "다산관2", 10);
        graph.addEdge("다산관", "다산관3", 37);
        graph.addEdge("우정당", "우정당1", 20);
        graph.addEdge("우정당", "우정당2", 13);
        graph.addEdge("94", "2", 41);
        graph.addEdge("후문", "94", 61);
        graph.addEdge("2", "3", 99.87);
        graph.addEdge("2", "11", 38.72);
        graph.addEdge("3", "4", 20.14);
        graph.addEdge("4", "5", 29.15);
        graph.addEdge("4", "24", 131.94);
        graph.addEdge("5", "7", 28.27);
        graph.addEdge("8", "7", 36);
        graph.addEdge("5", "8", 36.12);
        graph.addEdge("87", "6", 19.23);
        graph.addEdge("7", "87", 24.53);
        graph.addEdge("6", "9", 24.53);
        graph.addEdge("6", "13", 19.92);
        graph.addEdge("87", "11", 22.76);
        graph.addEdge("7", "9", 19.27);
        graph.addEdge("9", "10", 19.92);
        graph.addEdge("11", "14", 40.03);
        graph.addEdge("9", "8", 24.77);
        graph.addEdge("8", "12", 16.72);
        graph.addEdge("12", "10", 23.34);
        graph.addEdge("10", "13", 24.53);
        graph.addEdge("13", "15", 32.19);
        graph.addEdge("13", "14", 24.05);
        graph.addEdge("14", "16", 29.11);
        graph.addEdge("16", "15", 23.49);
        graph.addEdge("16", "30", 10.91);
        graph.addEdge("15", "17", 22.96);
        graph.addEdge("30", "29", 48.86);
        graph.addEdge("30", "18", 15.43);
        graph.addEdge("17", "18", 24.64);
        graph.addEdge("17", "21", 26.26);
        graph.addEdge("29", "28", 31.18);
        graph.addEdge("19", "88", 47.65);
        graph.addEdge("88", "28", 7.53);
        graph.addEdge("29", "88", 28.43);
        graph.addEdge("18", "19", 11.20);
        graph.addEdge("19", "20", 14.32);
        graph.addEdge("20", "21", 25.36);
        graph.addEdge("20", "27", 71.32);
        graph.addEdge("21", "22", 18.94);
        graph.addEdge("22", "23", 43.39);
        graph.addEdge("22", "26", 70.87);
        graph.addEdge("25", "26", 39.09);
        graph.addEdge("26", "80", 17.77);
        graph.addEdge("80", "27", 28.90);
        graph.addEdge("80", "81", 4.26);
        graph.addEdge("27", "37", 48.64);
        graph.addEdge("28", "31", 85.70);
        graph.addEdge("28", "36", 25.02);
        graph.addEdge("31", "32", 76.48);
        graph.addEdge("32", "33", 52.58);
        graph.addEdge("32", "41", 31.37);
        graph.addEdge("36", "35", 30.38);
        graph.addEdge("36", "37", 40.00);
        graph.addEdge("35", "39", 36.57);
        graph.addEdge("35", "34", 25.96);
        graph.addEdge("34", "38", 36.10);
        graph.addEdge("38", "39", 24.28);
        graph.addEdge("38", "41", 17.09);
        graph.addEdge("41", "42", 10.39);
        graph.addEdge("39", "40", 27.06);
        graph.addEdge("40", "49", 16.52);
        graph.addEdge("40", "44", 22.83);
        graph.addEdge("42", "43", 33.00);
        graph.addEdge("49", "37", 14.59);
        graph.addEdge("49", "50", 43.66);
        graph.addEdge("44", "48", 18.04);
        graph.addEdge("44", "43", 12.50);
        graph.addEdge("43", "45", 19.30);
        graph.addEdge("45", "46", 11.12);
        graph.addEdge("45", "51", 35.74);
        graph.addEdge("45", "47", 20.56);
        graph.addEdge("47", "48", 5.76);
        graph.addEdge("47", "65", 30.35);
        graph.addEdge("47", "62", 44.38);
        graph.addEdge("48", "50", 15.90);
        graph.addEdge("50", "65", 28.22);
        graph.addEdge("65", "66", 4.63);
        graph.addEdge("65", "64", 21.62);
        graph.addEdge("65", "68", 31.90);
        graph.addEdge("66", "69", 31.63);
        graph.addEdge("69", "70", 40.94);
        graph.addEdge("62", "51", 26.60);
        graph.addEdge("62", "70", 34.34);
        graph.addEdge("64", "63", 7.64);
        graph.addEdge("64", "67", 8.11);
        graph.addEdge("80", "81", 4.26);
        graph.addEdge("67", "73", 65);
        graph.addEdge("73", "84", 26.50);
        graph.addEdge("73", "쪽문3", 24.41);
        graph.addEdge("84", "대양AI센터1", 5.41);
        graph.addEdge("84", "81", 27.96);
        graph.addEdge("82", "83", 4.00);
        graph.addEdge("81", "82", 11.11);
        graph.addEdge("51", "61", 40.55);
        graph.addEdge("51", "33", 57.08);
        graph.addEdge("33", "쪽문1", 9.38);
        graph.addEdge("쪽문1", "53", 57.38);
        graph.addEdge("53", "93", 8);
        graph.addEdge("54", "93", 31);
        graph.addEdge("55", "93", 19);
        graph.addEdge("55", "56", 32.00);
        graph.addEdge("55", "59", 31.99);
        graph.addEdge("54", "56", 29.94);
        graph.addEdge("56", "57", 28.69);
        graph.addEdge("54", "쪽문2", 15.74);
        graph.addEdge("59", "57", 34.97);
        graph.addEdge("57", "58", 5.27);
        graph.addEdge("59", "61", 19.85);
        graph.addEdge("61", "71", 49.03);
        graph.addEdge("71", "70", 35.26);
        graph.addEdge("71", "86", 31.87);
        graph.addEdge("77", "78", 36.49);
        graph.addEdge("정문", "쪽문3", 137);
        graph.addEdge("79", "78", 7.49);
        graph.addEdge("78", "77", 25.79);
        graph.addEdge("76", "75", 25.84);
        graph.addEdge("75", "쪽문3", 15.00);
        graph.addEdge("75", "77", 82.00);
        graph.addEdge("90", "70", 11);
        graph.addEdge("충무관1", "5", 27);
        graph.addEdge("충무관1", "12", 29);
        graph.addEdge("90", "정문", 23);
        graph.addEdge("89", "충무관2", 2.79);
        graph.addEdge("7", "영실관", 17.53);
        graph.addEdge("5", "영실관", 22);
        graph.addEdge("10", "율곡관", 12.22);
        graph.addEdge("17", "학술정보원", 13.88);
        graph.addEdge("우정당1", "24", 9.76);
        graph.addEdge("우정당2", "23", 22.87);
        graph.addEdge("29", "용덕관1", 17.52);
        graph.addEdge("30", "용덕관1", 31.62);
        graph.addEdge("14", "16", 29);
        graph.addEdge("88", "진관홀", 10.21);
        graph.addEdge("29", "진관홀", 18.07);
        graph.addEdge("31", "이당관", 6.12);
        graph.addEdge("광개토관1", "35", 11);
        graph.addEdge("광개토관2", "34", 3);
        graph.addEdge("애지헌", "36", 12);
        graph.addEdge("군자관2", "46", 1);
        graph.addEdge("군자관1", "32", 19);
        graph.addEdge("군자관1", "41", 18);
        graph.addEdge("집현관1", "33", 6);
        graph.addEdge("집현관2", "61", 9);
        graph.addEdge("집현관3", "93", 9);
        graph.addEdge("집현관3", "55", 25);
        graph.addEdge("대양홀", "71", 1);
        graph.addEdge("모차르트홀", "58", 0);
        graph.addEdge("세종관1", "50", 1);
        graph.addEdge("세종관2", "91", 1);
        graph.addEdge("37", "91", 43);
        graph.addEdge("63", "91", 29.5);
        graph.addEdge("박물관", "92", 9);
        graph.addEdge("23", "92", 40);
        graph.addEdge("25", "92", 30);
        graph.addEdge("89", "24", 66);
        graph.addEdge("대양AI센터2", "76", 1);
        graph.addEdge("대양AI센터3", "79", 1);
        graph.addEdge("대양AI센터4", "83", 1);
        graph.addEdge("다산관2", "89", 10);
        graph.addEdge("다산관1", "24", 10);
        graph.addEdge("다산관3", "4", 15);
    }


}

