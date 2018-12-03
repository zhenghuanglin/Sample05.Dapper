
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample05
{
    public class Comment
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int cmt_id { get; set; }
        /// <summary>
        /// 文章id
        /// </summary>
        public int content_id { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string cmt_content { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime cmt_add_time { get; set; } = DateTime.Now;

    }
}
