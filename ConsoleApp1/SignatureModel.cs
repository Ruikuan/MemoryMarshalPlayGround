using System.ComponentModel;

namespace ConsoleApp1
{
    public class SignatureModel
    {
        [DisplayName("game_id")]
        public int UserAppId { get; set; }

        [DisplayName("auth_type")]
        public int AuthType { get; set; }

        [DisplayName("source")]
        public int Source { get; set; }

        [DisplayName("scene_id")]
        public string SceneId { get; set; }

        [DisplayName("need_tag")]
        public int NeedTag { get; set; }
    }
}
