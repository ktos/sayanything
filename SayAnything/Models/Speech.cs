using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.Speech.Synthesis;

namespace Ktos.SayAnything.Models
{
    class Speech
    {
        public static async void Say(string text, string language, VoiceGender gender)
        {
            var text2speech = new SpeechSynthesizer();
            var ssml = String.Format("<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"{0}\"><voice gender=\"{1}\">{2}</voice></speak>", language, gender.ToString(), text);
            await text2speech.SpeakSsmlAsync(ssml);            
        }

        public static IEnumerable<VoiceInformation> GetVoices()
        {            
            return InstalledVoices.All;
        }

        /*async void DoSomething()
        {
            foreach (var voice in InstalledVoices.All)
            {                
                using (var text2speech = new SpeechSynthesizer())
                {
                    text2speech.SetVoice(voice);
                    await text2speech.SpeakTextAsync("Hello world! I'm " + voice.DisplayName + ".");
                }
            }
        }*/
    }
}
