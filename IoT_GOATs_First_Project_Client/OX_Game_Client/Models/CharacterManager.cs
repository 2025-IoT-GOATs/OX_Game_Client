using CommunityToolkit.Mvvm.ComponentModel;
using OX_Game_Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OX_Game_Client.Models
{
    public class CharacterManager
    {
        private ObservableCollection<Character> participants = new ObservableCollection<Character>();
        public ObservableCollection<Character> Participants { get => participants;}

        private static CharacterManager instance;
        public static CharacterManager Instance { 
            get => instance; 
            set => instance = value; 
        }

        private CharacterManager(Character character)
        {
            Participants.Add(character);
        }
        //private CharacterManager()
        //{
        //}

        public static void InitCharManager(Character character)
        {
            if (Instance == null)
            {
                Instance = new CharacterManager(character);
            }
        }

        public void AddParticipant(Character character)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Participants.Add(character);
            });
        }
    }
}
