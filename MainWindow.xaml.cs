using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;


namespace CardMatching
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameState gameState;
        private Button firstCard;
        private Button secondCard;
        private bool isChecking;
        private MediaPlayer clickSound;
        private MediaPlayer correctMatch;
        private MediaPlayer gameOverSound;
        private MediaPlayer winSound;
        private DispatcherTimer timer;
        private TimeSpan gameTime;
        private int currentPair = 0;
        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
            InitializeSound();
            InitializeTimer();
        }

        private void InitializeGame()
        {
            gameState = new GameState();
            LoadCards();
           
        }

        private void LoadCards()
        {
            var cards=gameState.GetCards();
            var buttons = new List<Button>
            {
                 Button00, Button01,Button02,Button03,
                 Button10, Button11,Button12,Button13,
                 Button20 ,Button21,Button22,Button23,
                 Button30, Button31,Button32,Button33,

            };

            var shuffledCArds= cards.OrderBy(x => Guid.NewGuid()).ToList();

            for (int i = 0; i < buttons.Count; i++) 
            {
                var button = buttons[i];
                var card=shuffledCArds[i];
                button.Tag = card;
                button.Content = new System.Windows.Controls.Image
                {
                    Source = new BitmapImage(new Uri("C:\\Users\\oguzh\\source\\repos\\Yeni klasör\\Images\\letter.png")), 
                    Stretch = System.Windows.Media.Stretch.Fill
                };
            }
        }

        private void InitializeSound()
        {
            clickSound = new MediaPlayer();
            clickSound.Open(new Uri("C:\\Users\\oguzh\\source\\repos\\Yeni klasör\\Images\\Sounds\\mixkit-select-click-1109.wav"));

            correctMatch = new MediaPlayer();
            correctMatch.Open(new Uri("C:\\Users\\oguzh\\source\\repos\\Yeni klasör\\Images\\Sounds\\mixkit-correct-answer-reward-952.wav"));

            gameOverSound = new MediaPlayer();
            gameOverSound.Open(new Uri("C:\\Users\\oguzh\\source\\repos\\Yeni klasör\\Images\\Sounds\\mixkit-player-losing-or-failing-2042.wav"));

            winSound = new MediaPlayer();
            winSound.Open(new Uri("C:\\Users\\oguzh\\source\\repos\\Yeni klasör\\Images\\Sounds\\mixkit-game-level-completed-2059.wav"));
        }
        private void UpdateTimerDisplay()
        {
            TimerLabel.Content = gameTime.ToString(@"mm\:ss");
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            gameTime = gameTime.Subtract(TimeSpan.FromSeconds(1));

            if (gameTime.TotalSeconds <= 0)
            {
                timer.Stop();
                UnsuccessfulGameOverSound();
                var result=MessageBox.Show("Game Over", "Time is Up");
                if (result == MessageBoxResult.OK)
                {
                    ResetGame();
                    MenuScreen.Visibility = Visibility.Visible;
                    GameScreen.Visibility = Visibility.Collapsed;
                }

            }
            UpdateTimerDisplay();

        }

        private void SuccesfulGameOver()
        {
            
                timer.Stop();
                SuccesfulGameOverSound();
                var result1=MessageBox.Show("YOU WİN!");
            if (result1 == MessageBoxResult.OK)
            {
                ResetGame();
                MenuScreen.Visibility = Visibility.Visible;
                GameScreen.Visibility = Visibility.Collapsed;
            }

        }
        

        private void InitializeTimer()
        {
            gameTime = TimeSpan.FromSeconds(60);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
           
        }
        private void PlayClickSound()
        {
            clickSound.Stop();
            clickSound.Play();
        }
        private void PlayCorrectSound()
        {
            correctMatch.Stop();
            correctMatch.Play();
        }
        private void UnsuccessfulGameOverSound()
        {
            gameOverSound.Stop();
            gameOverSound.Play();          
        }
        private void SuccesfulGameOverSound()
        {
            winSound.Stop();
            winSound.Play();
        }
        private void Card_Click(object sender, RoutedEventArgs e)
        {
            if (isChecking==true) return;

            Button clickedButton = sender as Button;
            if (clickedButton == null) return;

            Card clickedCard = clickedButton.Tag as Card;
            if (clickedCard == null) return;

            // Kartın ön yüzünü göster
            ((System.Windows.Controls.Image)clickedButton.Content).Source = new BitmapImage(new Uri(clickedCard.ImagePath));

            PlayClickSound();

            if (firstCard == null)
            {
                firstCard = clickedButton;
            }
            else
            {
                secondCard = clickedButton;
                isChecking = true;

                // Kartları karşılaştır
                CheckForMatch();
            }
        }

        private async void CheckForMatch()
        {
            var _firstCard = firstCard.Tag as Card;
            var _secondCard = secondCard.Tag as Card;

            if (_firstCard.ImagePath == _secondCard.ImagePath)
            {
                // Eşleşti
                _firstCard.IsMatched = true;
                _secondCard.IsMatched = true;
                PlayCorrectSound();
                currentPair++;
                if (currentPair >= 8) 
                {
                    SuccesfulGameOver();
                }
            }
            else
            {
                // Eşleşmedi
                await Task.Delay(800); // Kartları 0,8 saniye göster
                ((System.Windows.Controls.Image)firstCard.Content).Source = new BitmapImage(new Uri("C:\\Users\\oguzh\\source\\repos\\Yeni klasör\\Images\\letter.png"));
                ((System.Windows.Controls.Image)secondCard.Content).Source = new BitmapImage(new Uri("C:\\Users\\oguzh\\source\\repos\\Yeni klasör\\Images\\letter.png"));
            }

            firstCard = null;
            secondCard = null;
            isChecking = false;
        }

        private void ResetGame()
        {
            firstCard = null;
            secondCard = null;
            isChecking = false;
            currentPair = 0;
            gameState = new GameState();
            LoadCards();
            SetGameTime();
            UpdateTimerDisplay();
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            MenuScreen.Visibility = Visibility.Collapsed;
            GameScreen.Visibility = Visibility.Visible;
            ResetGame();
            LoadCards();
            timer.Start();
            
            

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void TimerCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Only allow one checkbox to be checked at a time
            if (sender == Timer30Sec)
            {
                Timer1Min.IsChecked = false;
                Timer2Min.IsChecked = false;
            }
            else if (sender == Timer1Min)
            {
                Timer30Sec.IsChecked = false;
                Timer2Min.IsChecked = false;
            }
            else if (sender == Timer2Min)
            {
                Timer30Sec.IsChecked = false;
                Timer1Min.IsChecked = false;
            }
        }

        private void TimerCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // If none are checked, default to 1 minute
            if (!(Timer30Sec.IsChecked ?? false) && !(Timer1Min.IsChecked ?? false) && !(Timer2Min.IsChecked ?? false))
            {
                Timer1Min.IsChecked = true;
            }
        }

        private void SetGameTime()
        {
            if (Timer30Sec.IsChecked ?? false)
            {
                gameTime = TimeSpan.FromSeconds(30);
            }
            else if (Timer1Min.IsChecked ?? false)
            {
                gameTime = TimeSpan.FromSeconds(60);
            }
            else if (Timer2Min.IsChecked ?? false)
            {
                gameTime = TimeSpan.FromSeconds(120);
            }
            else
            {
                gameTime = TimeSpan.FromSeconds(60); // Default to 1 minute
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
            MenuScreen.Visibility = Visibility.Visible;
            GameScreen.Visibility = Visibility.Collapsed;

        }
    }
}
    

