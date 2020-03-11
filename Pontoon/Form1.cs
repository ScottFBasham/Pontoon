using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pontoon
{
    public partial class frmPontoon : Form
    {
        string[] suitNames = new string[4] { "Club", "Diamond", "Heart", "Spade" };
        PictureBox[] picSource = new PictureBox[4];
        PlayingCard[] deck = new PlayingCard[52];
        GroupBox[] playerGrpBox = new GroupBox[5];
        GroupBox[] bankerGrpBox = new GroupBox[5];
        PictureBox[] playerPic = new PictureBox[5];
        PictureBox[] bankerPic = new PictureBox[5];
        Label[] playerCard = new Label[5];
        Label[] bankerCard = new Label[5];
        int[] playerHand = new int[5] { -1, -1, -1, -1, -1 }; // -1 signifies "no card present"
        int[] dealerHand = new int[5] { -1, -1, -1, -1, -1 }; // -1 signifies "no card present"
        int numberOfPlayerCards = 0;
        int numberOfDealerCards = 0;
        bool playerPlaying = true;

        Random myRandom = new Random();
        int deckPosition = 0;
        

        public frmPontoon()
        {
            InitializeComponent();
        }

        private void frmPontoon_Load(object sender, EventArgs e)
        {
            // initialise deck
            for (int i = 0; i < 52; i++)
            {
                deck[i] = new PlayingCard(i);
            }
            playerGrpBox[0] = grpPCard1;
            playerGrpBox[1] = grpPCard2;
            playerGrpBox[2] = grpPCard3;
            playerGrpBox[3] = grpPCard4;
            playerGrpBox[4] = grpPCard5;
            bankerGrpBox[0] = grpBCard1;
            bankerGrpBox[1] = grpBCard2;
            bankerGrpBox[2] = grpBCard3;
            bankerGrpBox[3] = grpBCard4;
            bankerGrpBox[4] = grpBCard5;
            playerPic[0] = picPCard1;
            playerPic[1] = picPCard2;
            playerPic[2] = picPCard3;
            playerPic[3] = picPCard4;
            playerPic[4] = picPCard5;
            bankerPic[0] = picBCard1;
            bankerPic[1] = picBCard2;
            bankerPic[2] = picBCard3;
            bankerPic[3] = picBCard4;
            bankerPic[4] = picBCard5;
            playerCard[0] = lblPCard1;
            playerCard[1] = lblPCard2;
            playerCard[2] = lblPCard3;
            playerCard[3] = lblPCard4;
            playerCard[4] = lblPCard5;
            bankerCard[0] = lblBCard1;
            bankerCard[1] = lblBCard2;
            bankerCard[2] = lblBCard3;
            bankerCard[3] = lblBCard4;
            bankerCard[4] = lblBCard5;

            picSource[0] = picClub;
            picSource[1] = picDiamond;
            picSource[2] = picHeart;
            picSource[3] = picSpade;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            PlayingCard singleCard = new PlayingCard((int)numericUpDown1.Value);
            textBox1.Clear();
            textBox1.Text += singleCard.display+ " of " + suitNames[singleCard.suit]+"s\r\n";
            textBox1.Text += "  listItem is: " + singleCard.listItem + "\r\n";
            textBox1.Text += "  value is: " + singleCard.value + "\r\n";
        }



        private void btnListDeck_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            for (int i = 0; i < 52; i++)
            {
                textBox1.Text += deck[i].display + " of " + suitNames[deck[i].suit] + "s\r\n";
            }
        }

        private void btnShuffle_Click(object sender, EventArgs e)
        {
            for (int i=51; i>0; i--)
            {
                //swap a random card (located below i) with position i
                int swapPosition = myRandom.Next(i);
                PlayingCard temp = deck[i];
                deck[i] = deck[swapPosition];
                deck[swapPosition] = temp;
            }
            MessageBox.Show("Done");
        }

        private void shuffleDeck()
        {
            for (int i = 51; i > 0; i--)
            {
                //swap a random card (located below i) with position i
                int swapPosition = myRandom.Next(i);
                PlayingCard temp = deck[i];
                deck[i] = deck[swapPosition];
                deck[swapPosition] = temp;
            }
        }

        private void resetHands()
        {
            for (int i=0;i<5;i++)
            {
                playerHand[i] = -1;
                dealerHand[i] = -1;
                playerPic[i].Visible = false;
                bankerPic[i].Visible = false;
                playerCard[0].Visible = false;
                bankerCard[0].Visible = false;
            }
            numberOfPlayerCards = 0;
            numberOfDealerCards = 0;
            playerPlaying = false;
            btnPlayerStick.Enabled = false;
            btnPlayerDealOne.Enabled = false;
            btnBankStick.Enabled = false;
            btnBankDealOne.Enabled = false;
            btnDeal.Enabled = true;

        }

        private void btnDeal_Click(object sender, EventArgs e)
        {
            //deal two cards to Player
            assignCardToPlayer(deckPosition, 0);
            assignCardToPlayer(deckPosition+1, 1);
            deckPosition += 2;
        }

        private void assignCardToPlayer(int packIndex, int position)
        {
            playerPic[position].Image = picSource[deck[packIndex].suit].Image;
            playerCard[position].Text = deck[packIndex].display;
            playerPic[position].Visible = true;
            playerCard[position].Visible = true;
        }
    }
}
