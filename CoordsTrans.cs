using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OSGeo.GDAL;
using OSGeo.OGR;
using OSGeo.OSR;
namespace trans
{
    public static class CoordsTrans
    {
        /// <summary>
        /// 对wkt格式的linestring、multilinestring和polygon从web墨卡托投影转换为适用于高德、腾讯的经纬度坐标（GCJ-02)
        /// </summary>
        /// <param name="wkt"></param>
        /// <returns></returns>
        public static string Trans_3857toGCJ02(string wkt)
        {
            string[] result_param = { };
            string result__geojson = "";
            if (wkt.ToUpper().Contains("POLYGON"))
            {
                Geometry polygon = Geometry.CreateFromWkt(wkt);
                //定义数据源坐标系统
                SpatialReference spa_3857 = new SpatialReference(epsg.projcs_3857);
                //定义目标坐标系统
                SpatialReference spa_4326 = new SpatialReference(epsg.gcs_4326);
                polygon.AssignSpatialReference(spa_3857);
                //将几何面从3857转为4326坐标
                polygon.TransformTo(spa_4326);
                string[] polygon_str = { };
                //将几何面转为geojson字符串格式；
                string polygon_json = polygon.ExportToJson(polygon_str);
                //将字符串解析为json对象
                JObject result_json = JObject.Parse(polygon_json) as JObject;
                JArray result_polygon = result_json["coordinates"] as JArray;


                Geometry gcj02_polygon = new Geometry(wkbGeometryType.wkbPolygon);
                for (int i = 0; i < result_polygon.Count; i++)
                {
                    JArray result_lines = result_polygon[i] as JArray;
                    Geometry gcj02_line = new Geometry(wkbGeometryType.wkbLinearRing);
                    for (int j = 0; j < result_lines.Count; j++)
                    {
                        JArray p = result_lines[j] as JArray;
                        double wgs84_lng = System.Convert.ToDouble(p[0]);
                        double wgs84_lat = System.Convert.ToDouble(p[1]);
                        //将wgs84经纬度坐标转为gcj-02经纬度坐标
                        double[] gcj02_coord = Coordtransform.Wgs84togcj02(wgs84_lng, wgs84_lat);
                        double gcj02_lng = gcj02_coord[0];
                        double gcj02_lat = gcj02_coord[1];
                        //将点连接成线
                        gcj02_line.AddPoint_2D(gcj02_lng, gcj02_lat);
                    }
                    //将线连接成面
                    gcj02_polygon.AddGeometry(gcj02_line);
                }

                result__geojson = gcj02_polygon.ExportToJson(result_param);


            }
            if (wkt.ToUpper().Contains("MULTILINESTRING"))
            {
                Geometry polyline = Geometry.CreateFromWkt(wkt);
                //定义数据源坐标系统
                SpatialReference spa_3857 = new SpatialReference(wkt_3857);
                //定义目标坐标系统
                SpatialReference spa_4326 = new SpatialReference(wkt_4326);
                polyline.AssignSpatialReference(spa_3857);
                //将几何线从3857转为4326坐标
                polyline.TransformTo(spa_4326);
                string[] polyline_str = { };
                //将几何线转为geojson字符串格式；
                String polyline_json = polyline.ExportToJson(polyline_str);
                //将字符串解析为json对象
                JObject result_json = JObject.Parse(polyline_json) as JObject;
                JArray result_polyline = result_json["coordinates"] as JArray;
                Geometry gcj02_multiline = new Geometry(wkbGeometryType.wkbMultiLineString);
                for (int i = 0; i < result_polyline.Count; i++)
                {
                    JArray result_lines = result_polyline[i] as JArray;
                    Geometry gcj02_line = new Geometry(wkbGeometryType.wkbLineString);
                    for (int j = 0; j < result_lines.Count; j++)
                    {
                        JArray p = result_lines[j] as JArray;
                        double wgs84_lng = System.Convert.ToDouble(p[0]);
                        double wgs84_lat = System.Convert.ToDouble(p[1]);
                        //将wgs84经纬度坐标转为gcj-02经纬度坐标
                        double[] gcj02_coord = Coordtransform.Wgs84togcj02(wgs84_lng, wgs84_lat);
                        double gcj02_lng = gcj02_coord[0];
                        double gcj02_lat = gcj02_coord[1];
                        //将点连接成线
                        gcj02_line.AddPoint_2D(gcj02_lng, gcj02_lat);
                    }
                    //将线连接成多线
                    gcj02_multiline.AddGeometry(gcj02_line);
                }
                result__geojson = gcj02_multiline.ExportToJson(result_param);

            }
            if (wkt.ToUpper().Contains("LINESTRING"))
            {
                Geometry linestring = Geometry.CreateFromWkt(wkt);
                //定义数据源坐标系统
                SpatialReference spa_3857 = new SpatialReference(wkt_3857);
                //定义目标坐标系统
                SpatialReference spa_4326 = new SpatialReference(wkt_4326);
                linestring.AssignSpatialReference(spa_3857);
                //将几何线从3857转为4326坐标
                linestring.TransformTo(spa_4326);
                string[] linestring_str = { };
                //将几何线转为geojson字符串格式；
                String linestring_json = linestring.ExportToJson(linestring_str);
                //将字符串解析为json对象
                JObject result_json = JObject.Parse(linestring_json) as JObject;
                JArray result_polyline = result_json["coordinates"] as JArray;
                Geometry gcj02_line = new Geometry(wkbGeometryType.wkbLineString);
                for (int i = 0; i < result_polyline.Count; i++)
                {
                    JArray p = result_polyline[i] as JArray;
                    double wgs84_lng = System.Convert.ToDouble(p[0]);
                    double wgs84_lat = System.Convert.ToDouble(p[1]);
                    //将wgs84经纬度坐标转为gcj02经纬度坐标
                    double[] gcj02_coord = Coordtransform.Wgs84togcj02(wgs84_lng, wgs84_lat);
                    double gcj02_lng = gcj02_coord[0];
                    double gcj02_lat = gcj02_coord[1];
                    //将点连接成线
                    gcj02_line.AddPoint_2D(gcj02_lng, gcj02_lat);


                }
                result__geojson = gcj02_line.ExportToJson(result_param);


            }
            return result__geojson;

        }
        /// <summary>
        /// 对wkt格式的linestring、multilinestring和polygon从web墨卡托投影转换为适用于百度坐标系的BD-09经纬度坐标
        /// </summary>
        /// <param name="wkt"></param>
        /// <returns></returns>
        public static string Trans_3857toBD09(string wkt)
        {
            string[] result_param = { };
            string result__geojson = "";
            if (wkt.ToUpper().Contains("POLYGON"))
            {
                Geometry polygon = Geometry.CreateFromWkt(wkt);
                //定义数据源坐标系统
                SpatialReference spa_3857 = new SpatialReference(wkt_3857);
                //定义目标坐标系统
                SpatialReference spa_4326 = new SpatialReference(wkt_4326);
                polygon.AssignSpatialReference(spa_3857);
                //将几何面从3857转为4326坐标
                polygon.TransformTo(spa_4326);
                string[] polygon_str = { };
                //将几何面转为geojson字符串格式；
                string polygon_json = polygon.ExportToJson(polygon_str);
                //将字符串解析为json对象
                JObject result_json = JObject.Parse(polygon_json) as JObject;
                JArray result_polygon = result_json["coordinates"] as JArray;


                Geometry bd09_polygon = new Geometry(wkbGeometryType.wkbPolygon);
                for (int i = 0; i < result_polygon.Count; i++)
                {
                    JArray result_lines = result_polygon[i] as JArray;
                    Geometry bd09_line = new Geometry(wkbGeometryType.wkbLinearRing);
                    for (int j = 0; j < result_lines.Count; j++)
                    {
                        JArray p = result_lines[j] as JArray;
                        double wgs84_lng = System.Convert.ToDouble(p[0]);
                        double wgs84_lat = System.Convert.ToDouble(p[1]);
                        //将wgs84经纬度坐标转为BD-09经纬度坐标
                        double[] bd09_coord = Coordtransform.Wgs84tobd09(wgs84_lng, wgs84_lat);
                        double bd09_lng = bd09_coord[0];
                        double bd09_lat = bd09_coord[1];
                        //将点连接成线
                        bd09_line.AddPoint_2D(bd09_lng, bd09_lat);
                    }
                    //将线连接成面
                    bd09_polygon.AddGeometry(bd09_line);
                }

                result__geojson = bd09_polygon.ExportToJson(result_param);


            }
            if (wkt.ToUpper().Contains("MULTILINESTRING"))
            {
                Geometry polyline = Geometry.CreateFromWkt(wkt);
                //定义数据源坐标系统
                SpatialReference spa_3857 = new SpatialReference(wkt_3857);
                //定义目标坐标系统
                SpatialReference spa_4326 = new SpatialReference(wkt_4326);
                polyline.AssignSpatialReference(spa_3857);
                //将几何线从3857转为4326坐标
                polyline.TransformTo(spa_4326);
                string[] polyline_str = { };
                //将几何线转为geojson字符串格式；
                String polyline_json = polyline.ExportToJson(polyline_str);
                //将字符串解析为json对象
                JObject result_json = JObject.Parse(polyline_json) as JObject;
                JArray result_polyline = result_json["coordinates"] as JArray;
                Geometry bd09_multiline = new Geometry(wkbGeometryType.wkbMultiLineString);
                for (int i = 0; i < result_polyline.Count; i++)
                {
                    JArray result_lines = result_polyline[i] as JArray;
                    Geometry bd09_line = new Geometry(wkbGeometryType.wkbLineString);
                    for (int j = 0; j < result_lines.Count; j++)
                    {
                        JArray p = result_lines[j] as JArray;
                        double wgs84_lng = System.Convert.ToDouble(p[0]);
                        double wgs84_lat = System.Convert.ToDouble(p[1]);
                        //将wgs84经纬度坐标转为BD-09经纬度坐标
                        double[] bd09_coord = Coordtransform.Wgs84tobd09(wgs84_lng, wgs84_lat);
                        double bd09_lng = bd09_coord[0];
                        double bd09_lat = bd09_coord[1];
                        //将点连接成线
                        bd09_line.AddPoint_2D(bd09_lng, bd09_lat);
                    }
                    //将线连接成多线
                    bd09_multiline.AddGeometry(bd09_line);
                }
                result__geojson = bd09_multiline.ExportToJson(result_param);

            }
            if (wkt.ToUpper().Contains("LINESTRING"))
            {
                Geometry linestring = Geometry.CreateFromWkt(wkt);
                //定义数据源坐标系统
                SpatialReference spa_3857 = new SpatialReference(wkt_3857);
                //定义目标坐标系统
               SpatialReference spa_4326 = new SpatialReference(wkt_4326);
                linestring.AssignSpatialReference(spa_3857);
               //将几何线从3857转为4326坐标
                linestring.TransformTo(spa_4326);
                string[] linestring_str = { };
                //将几何线转为geojson字符串格式；
               String linestring_json = linestring.ExportToJson(linestring_str);
                //将字符串解析为json对象
                JObject result_json = JObject.Parse(linestring_json) as JObject;
               JArray result_polyline = result_json["coordinates"] as JArray;
                Geometry bd09_line = new Geometry(wkbGeometryType.wkbLineString);
                for (int i = 0; i < result_polyline.Count; i++)
                {
                        JArray p = result_polyline[i] as JArray;
                       double wgs84_lng = System.Convert.ToDouble(p[0]);
                        double wgs84_lat = System.Convert.ToDouble(p[1]);
                        //将wgs84经纬度坐标转为BD-09经纬度坐标
                        double[] bd09_coord = Coordtransform.Wgs84tobd09(wgs84_lng, wgs84_lat);
                        double bd09_lng = bd09_coord[0];
                        double bd09_lat = bd09_coord[1];
                    //将点连接成线
                    bd09_line.AddPoint_2D(bd09_lng, bd09_lat);


                }
                result__geojson = bd09_line.ExportToJson(result_param);


            }
            return result__geojson;
        }


    }
}
