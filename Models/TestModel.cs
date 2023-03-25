using Newtonsoft.Json;

namespace platzi_blobConsole
{
    public class TestModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }


        [JsonProperty("desc")]
        public string Descripcion { get; set; }
    }
}



