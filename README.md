# geotrans
一、What can do?

 1.将3857坐标的wkt格式的polygon/multilinestring/linestring转为gcj-02的geojson格式的字符串
 
 2.将3857坐标的wkt格式的polygon/multilinestring/linestring转为bd-09的geojson格式的字符串
 
二、How to use?
 1. CoordsTrans.Trans_3857toGCJ02(string WKT);
 2. CoordsTrans.Trans_3857toBD09(string WKT);
 
 1)Parameter WKT:wkt format(polygon/multilinestring/linestring)
 
 exmaple:WKT="LINESTRING(13746967.180598299950361 5125687.142056070268154,13746967.372486900538206 5125687.087575890123844)";
         
 2)Return:geojson(string)
 
 example:{ "type": "MultiLineString", "coordinates": [ [ [ 101.859862859712493, 36.523079718151706 ], [ 101.859456035152945,   36.523369313600973 ] ] ] };
 { "type": "MultiLineString", "coordinates": [ [ [ 101.858121153728945, 36.523341793361269 ], [ 101.857713408838165, 36.52363062430571 ] ] ] };

三、Note：
 相关依赖：GDAL  可参见该博客进行GDAL在vs2017中的配置 https://blog.csdn.net/xlp789/article/details/89521110

