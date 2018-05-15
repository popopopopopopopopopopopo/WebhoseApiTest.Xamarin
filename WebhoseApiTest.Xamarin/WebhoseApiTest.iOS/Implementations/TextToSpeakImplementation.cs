using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebhoseApiTest.Interfaces;
using Foundation;
using UIKit;
using AVFoundation;
using Xamarin.Forms;
using WebhoseApiTest.iOS.Implementations;

[assembly: Dependency(typeof(TextToSpeakImplementation))]
namespace WebhoseApiTest.iOS.Implementations
{
    public class TextToSpeakImplementation : ITextToSpeak
    {
        public TextToSpeakImplementation() { }

        public void Speak(string text)
        {
            var speechSynthesizer = new AVSpeechSynthesizer();

        }
    }
}