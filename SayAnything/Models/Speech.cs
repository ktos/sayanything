using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.Phone.Speech.Synthesis;

namespace Ktos.SayAnything.Models
{
    class Speech
    {
        public static IAsyncAction Say(string text, string language, VoiceGender gender)
        {
            var text2speech = new SpeechSynthesizer();
            var ssml = string.Format("<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"{0}\"><voice gender=\"{1}\">{2}</voice></speak>", language, gender.ToString().ToLower(), text);
            return text2speech.SpeakSsmlAsync(ssml);            
        }

        public static IEnumerable<VoiceInformation> GetVoices()
        {            
            return InstalledVoices.All;
        }

        public static string GetDefaultVoiceId()
        {
            return InstalledVoices.Default.Id;
        }

        public static VoiceInformation GetVoice(string id)
        {
            return (from f in InstalledVoices.All where f.Id == id select f).Single();
        }        
    }
}
