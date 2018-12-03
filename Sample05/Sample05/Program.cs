using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Sample05
{
    class Program
    {
        static string connStr = "Data Source=.;User ID=sa;Password=sql@123456;Initial Catalog=testdb;Pooling=true;Max Pool Size=100;";

        static void Main(string[] args)
        {
            //test_insert();
            //test_mult_insert();

            //test_del();
            //test_mult_del();

            //test_update();
            //test_mult_update();

            //test_select_one();
            //test_select_list();

            test_select_content_with_comment();


            //Console.WriteLine("Hello World!");
            Console.ReadLine();
        }

        /// <summary>
        /// 测试插入单条数据
        /// </summary>
        static void test_insert()
        {
            var content = new Content
            {
                title = "标题2",
                content = "内容2",

            };
            using (var conn = new SqlConnection(connStr))
            {
                string sql_insert = @"INSERT INTO [Content]
                (title, [content], status, add_time, modify_time)
VALUES   (@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_insert：插入了{result}条数据！");
            }
        }

        /// <summary>
        /// 测试一次批量插入两条数据
        /// </summary>
        static void test_mult_insert()
        {
            List<Content> contents = new List<Content>() {
               new Content
            {
                title = "批量插入标题1",
                content = "批量插入内容1",

            },
               new Content
            {
                title = "批量插入标题2",
                content = "批量插入内容2",

            },
        };

            using (var conn = new SqlConnection(connStr))
            {
                string sql_insert = @"INSERT INTO [Content]
                (title, [content], status, add_time, modify_time)
VALUES   (@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, contents);
                Console.WriteLine($"test_mult_insert：插入了{result}条数据！");
            }
        }

        /// <summary>
        /// 测试删除单条数据
        /// </summary>
        static void test_del()
        {
            var content = new Content
            {
                id = 2,

            };
            using (var conn = new SqlConnection(connStr))
            {
                string sql_insert = @"DELETE FROM [Content]
WHERE   (id = @id)";
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_del：删除了{result}条数据！");
            }
        }

        /// <summary>
        /// 测试一次批量删除两条数据
        /// </summary>
        static void test_mult_del()
        {
            List<Content> contents = new List<Content>() {
               new Content
            {
                id=3,

            },
               new Content
            {
                id=4,

            },
        };

            using (var conn = new SqlConnection(connStr))
            {
                string sql_insert = @"DELETE FROM [Content]
WHERE   (id = @id)";
                var result = conn.Execute(sql_insert, contents);
                Console.WriteLine($"test_mult_del：删除了{result}条数据！");
            }
        }


            /// <summary>
            /// 测试修改单条数据
            /// </summary>
            static void test_update()
            {
                var content = new Content
                {
                    id = 5,
                    title = "标题5",
                    content = "内容5",

                };
                using (var conn = new SqlConnection(connStr))
                {
                    string sql_insert = @"UPDATE  [Content]
SET         title = @title, [content] = @content, modify_time = GETDATE()
WHERE   (id = @id)";
                    var result = conn.Execute(sql_insert, content);
                    Console.WriteLine($"test_update：修改了{result}条数据！");
                }
            }

            /// <summary>
            /// 测试一次批量修改多条数据
            /// </summary>
            static void test_mult_update()
            {
                List<Content> contents = new List<Content>() {
               new Content
            {
                id=6,
                title = "批量修改标题6",
                content = "批量修改内容6",

            },
               new Content
            {
                id =7,
                title = "批量修改标题7",
                content = "批量修改内容7",

            },
        };

                using (var conn = new SqlConnection(connStr))
                {
                    string sql_insert = @"UPDATE  [Content]
SET         title = @title, [content] = @content, modify_time = GETDATE()
WHERE   (id = @id)";
                    var result = conn.Execute(sql_insert, contents);
                    Console.WriteLine($"test_mult_update：修改了{result}条数据！");
                }
            }

        /// <summary>
        /// 查询单条指定的数据
        /// </summary>
        static void test_select_one()
        {
            using (var conn = new SqlConnection(connStr))
            {
                string sql_insert = @"select * from [dbo].[content] where id=@id";
                var result = conn.QueryFirstOrDefault<Content>(sql_insert, new { id = 5 });
                Console.WriteLine($"test_select_one：查到的数据为：");
            }
        }

        /// <summary>
        /// 查询多条指定的数据
        /// </summary>
        static void test_select_list()
        {
            using (var conn = new SqlConnection(connStr))
            {
                string sql_insert = @"select * from [dbo].[content] where id in @ids";
                var result = conn.Query<Content>(sql_insert, new { ids = new int[] { 6, 7 } });
                Console.WriteLine($"test_select_one：查到的数据为：");
            }
        }

        static void test_select_content_with_comment()
        {
            List<Content> contentList = new List<Content>();

            var lookUp = new Dictionary<int, Content>();

            using (var conn = new SqlConnection(connStr))
            {
                string sqlCommandText3 = @"SELECT a.*,b.*
                FROM   dbo.Content a WITH(NOLOCK)
                       LEFT JOIN dbo.Comment b
                            ON  b.content_id = a.id";

                contentList = conn.Query<Content, Comment, Content>(sqlCommandText3,
                    (cnt, cmt) =>
                    {
                        Content u;
                        if (!lookUp.TryGetValue(cnt.id, out u))
                        {
                            lookUp.Add(cnt.id, u = cnt);
                        }
                        u.Comments.Add(cmt);
                        return cnt;
                    }, null, null, true, "cmt_id", null, null).ToList();
                var result = lookUp.Values;


            }
            if (lookUp.Count > 0)
            {
                lookUp.Values.ToList().ForEach((item) => Console.WriteLine("Title:" + item.title +
                                                             "----Content:" + item.content +
                                                             "-----Comments:" + item.Comments.Count +
                                                             "\n"));

                Console.ReadLine();
            }



        }


    }
}
