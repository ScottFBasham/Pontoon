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
        int[] bankerHand = new int[5] { -1, -1, -1, -1, -1 }; // -1 signifies "no card present"
        int numberOfPlayerCards = 0;
        int playerHandScore = 0;
        int bankerHandScore = 0;
        int playerVirtualScore = 0;
        int bankerVirtualScore = 0;
        int playerOverallScore = 0;
        int bankerOverallScore = 0;
        int numberOfBankerCards = 0;
        bool playerAcePresent = false;
        bool bankerAcePresent = false;
        bool playerPlaying = true;
        bool handFinishedFlag = false;

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

            shuffle();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            PlayingCard singleCard = new PlayingCard((int)numericUpDown1.Value);
            textBox1.Clear();
            textBox1.Text += singleCard.display + " of " + suitNames[singleCard.suit] + "s\r\n";
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
            for (int i = 51; i > 0; i--)
            {
                shuffle();
            }
            MessageBox.Show("Done");
        }

        private void resetHands()
        {
            for (int i = 0; i < 5; i++)
            {
                playerHand[i] = -1;
                bankerHand[i] = -1;
                playerPic[i].Visible = false;
                bankerPic[i].Visible = false;
                playerCard[i].Visible = false;
                bankerCard[i].Visible = false;  
            }
            numberOfPlayerCards = 0;
            numberOfBankerCards = 0;
            playerPlaying = false;
            btnPlayerStick.Enabled = false;
            btnPlayerDealOne.Enabled = false;
            btnBankerStick.Enabled = false;
            btnBankerDealOne.Enabled = false;
            btnDeal.Enabled = true;
            playerAcePresent = false;
            bankerAcePresent = false;
            playerHandScore = 0;
            bankerHandScore = 0;
            playerVirtualScore = 0;
            bankerVirtualScore = 0;
            handFinishedFlag = false;
            lblOutcome.Visible = false;
            lblBankerHandScore.Text = "0";
            lblPlayerMessage.Visible = false;
            lblBankerMessage.Visible = false;
        }

        private void shuffle()
        {
            for (int i = 51; i > 0; i--)
            {
                //swap a random card (located below i) with position i
                int swapPosition = myRandom.Next(i);
                PlayingCard temp = deck[i];
                deck[i] = deck[swapPosition];
                deck[swapPosition] = temp;
            }
            deckPosition = 0;
        }

        private void btnDeal_Click(object sender, EventArgs e)
        {
            if (handFinishedFlag)
            {
                if (deckPosition > 36)
                {
                    MessageBox.Show((52 - deckPosition).ToString()
                        + " card(s) left - reshuffling the deck", "Reshuffle", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    shuffle();
                }
                resetHands();
            }

            //deal two cards to Player
            dealToPlayer();
            dealToPlayer();
            btnDeal.Enabled = false;
            btnPlayerDealOne.Enabled = true;
            btnPlayerStick.Enabled = true;
            playerPlaying = true;
        }

        private void btnBankerDealOne_Click(object sender, EventArgs e)
        {
            dealToBanker();
        }

        private void dealToBanker()
        {
            if (numberOfBankerCards == 5) return;
            assignCardToBanker(numberOfBankerCards);
        }

        private void assignCardToBanker(int position)
        {
            bankerPic[position].Image = picSource[deck[deckPosition].suit].Image;
            bankerCard[position].Text = deck[deckPosition].display;
            bankerPic[position].Visible = true;
            bankerCard[position].Visible = true;
            bankerHandScore += deck[deckPosition].value; // add to hand's total
            if (deck[deckPosition].listItem == 1) bankerAcePresent = true; // record Ace

            //bankerHand[position] = deck[deckPosition].indexNo; // record card in player's hand
            bankerHand[position] = deckPosition;

            deckPosition++; // next card from pack
            numberOfBankerCards++;
            lblNumLeft.Text = (52 - deckPosition).ToString();
            lblBankerHandScore.Text = bankerHandScore.ToString();
            if (bankerAcePresent) lblBankerHandScore.Text += " or " + (bankerHandScore + 10).ToString();
            //check to see if 5 card-trick
            if (numberOfBankerCards == 5)
            {
                lblBankerMessage.Text = "5 CARD TRICK";
                lblBankerMessage.Visible = true;
                btnBankerStick.Enabled = false;
                btnBankerDealOne.Enabled = false;
                btnDeal.Enabled = true;
                handFinished();
            }
            //check to see if bust
            if (bankerHandScore > 21)
            {
                lblBankerMessage.Text = "BUST";
                lblBankerMessage.Visible = true;
                btnBankerDealOne.Enabled = false;
                btnBankerStick.Enabled = false;
                btnDeal.Enabled = true;
                handFinished();
            }
        }

        private void handFinished()
        {
            handFinishedFlag = true;

            //calculate Player virtual score
            playerVirtualScore = playerHandScore;
            if (numberOfPlayerCards == 5) playerVirtualScore = 22; // 5 card trick beats an ordinary hand
            if (playerAcePresent && (numberOfPlayerCards == 2))
            {
                //potential pontoon - need to check the other non-ace card
                int otherCard = deck[playerHand[0]].listItem;
             /*   textBox1.Text = "Two cards with an ace - looking for pontoon\r\n";
                textBox1.Text += "First card listItem is:  " + deck[playerHand[0]].listItem + "\r\n";
                textBox1.Text += "  Suit: " + deck[playerHand[0]].suit + "\r\n";
                textBox1.Text += "  position in pack is "+playerHand[0]+ "\r\n";
                textBox1.Text += "Second card listItem is:  " + deck[playerHand[1]].listItem + "\r\n";
                textBox1.Text += "  Suit: " + deck[playerHand[1]].suit + "\r\n";
                textBox1.Text += "  position in pack is " + playerHand[1] + "\r\n";  */

                if (otherCard == 1) otherCard = deck[playerHand[1]].listItem;
                if (otherCard >= 10) // must be a pontoon
                {
                    playerVirtualScore = 13 + otherCard; // so a "10" gives a score of 23, Jack of 24, Queen of 25, King of 26
                }
            }
            if (playerHandScore > 21) playerVirtualScore = 0; // Player bust

            //calculate Banker virtual score
            if (playerVirtualScore ==0 )
            {
                bankerVirtualScore = 1; // player is bust so bank wins
            }
            else
            {
                bankerVirtualScore = bankerHandScore;
                if (numberOfBankerCards == 5) bankerVirtualScore = 22; // 5 card trick beats an ordinary hand
                if (bankerAcePresent && (numberOfBankerCards == 2))
                {
                    //potential pontoon -need to check the other non-ace card
                    int otherCard = deck[bankerHand[0]].listItem;
                    /*textBox1.Text = "Two cards with an ace - looking for pontoon\r\n";
                    textBox1.Text += "First card listItem is:  " + deck[bankerHand[0]].listItem + "\r\n";
                    textBox1.Text += "  Suit: " + deck[bankerHand[0]].suit + "\r\n";
                    textBox1.Text += "  position in pack is " + bankerHand[0] + "\r\n";
                    textBox1.Text += "Second card listItem is:  " + deck[bankerHand[1]].listItem + "\r\n";
                    textBox1.Text += "  Suit: " + deck[bankerHand[1]].suit + "\r\n";
                    textBox1.Text += "  position in pack is " + bankerHand[1] + "\r\n"; */
                    if (otherCard == 1) otherCard = deck[bankerHand[1]].listItem;
                    if (otherCard >= 10) // must be a pontoon
                    {
                        bankerVirtualScore = 13 + otherCard; // so a "10" gives a score of 23, Jack of 24, Queen of 25, King of 26
                    }
                }
                if (bankerHandScore > 21) bankerVirtualScore = 0; // Player bust
            }
            lblOutcome.Text = "Player: " + playerVirtualScore + " Banker: " + bankerVirtualScore;
            string outcome = "(a tie - no points)";
            if (playerVirtualScore > bankerVirtualScore)
            {
                outcome = "(Player wins)";
                if (playerVirtualScore > 22)
                {
                    // more points for a pontoon
                    playerOverallScore += 15;
                }
                else
                {
                    playerOverallScore += 10;
                }
            }
            else if (playerVirtualScore < bankerVirtualScore)
            {
                outcome = "(Banker wins)";
                if (bankerVirtualScore > 22)
                {
                    // more points for a pontoon
                    bankerOverallScore += 15;
                }
                else
                {
                    bankerOverallScore += 10;
                }
            }
            lblOutcome.Text += " " + outcome;
            lblOutcome.Visible = true;
            lblPlayerOverall.Text = playerOverallScore.ToString();
            lblBankerOverall.Text = bankerOverallScore.ToString();
        }

        private void dealToPlayer()
        {
            if (numberOfPlayerCards == 5) return;
            assignCardToPlayer(numberOfPlayerCards);
        }

        private void btnPlayerDealOne_Click(object sender, EventArgs e)
        {
            if ((!playerPlaying) || (numberOfPlayerCards == 5)) return;
            assignCardToPlayer(numberOfPlayerCards);
        }


        private void assignCardToPlayer(int position)
        {
            playerPic[position].Image = picSource[deck[deckPosition].suit].Image;
            playerCard[position].Text = deck[deckPosition].display;
            playerPic[position].Visible = true;
            playerCard[position].Visible = true;
            playerHandScore += deck[deckPosition].value; // add to hand's total
            if (deck[deckPosition].listItem == 1) playerAcePresent = true; // record Ace

            //playerHand[position] = deck[deckPosition].indexNo; // record card in player's hand
            playerHand[position] = deckPosition;

            textBox1.Text += "updating playerHand[] in index: " + position+" with value "+ deck[deckPosition].indexNo+"\r\n";

            deckPosition++; // next card from pack
            numberOfPlayerCards++;
            lblNumLeft.Text = (52 - deckPosition).ToString();
            lblPlayerHandScore.Text = playerHandScore.ToString();
            if (playerAcePresent) lblPlayerHandScore.Text += " or " + (playerHandScore + 10).ToString();
            //check to see if 5 card-trick
            if (numberOfPlayerCards == 5)
            {
                lblPlayerMessage.Text = "5 CARD TRICK";
                lblPlayerMessage.Visible = true;
                playerPlaying = false;
                btnPlayerStick.Enabled = false;
                btnPlayerDealOne.Enabled = false;
                btnBankerDealOne.Enabled = true;
                btnBankerStick.Enabled = true;
                dealToBanker();
                dealToBanker();
            }
                //check to see if bust
                if (playerHandScore > 21)
            {
                lblPlayerMessage.Text = "BUST";
                lblPlayerMessage.Visible = true;
                btnPlayerDealOne.Enabled = false;
                btnPlayerStick.Enabled = false;
                btnDeal.Enabled = true;
                handFinished();
            }
        }

        private void btnPlayerStick_Click(object sender, EventArgs e)
        {
            if (playerPlaying)
            {
                playerPlaying = false;
                btnPlayerStick.Enabled = false;
                btnPlayerDealOne.Enabled = false;
                btnBankerDealOne.Enabled = true;
                btnBankerStick.Enabled = true;
                dealToBanker();
                dealToBanker();
            }
            //calculate best score for player
            if (playerAcePresent && (playerHandScore < 12)) playerHandScore += 10;
            lblPlayerHandScore.Text = playerHandScore.ToString();
        }


        private void btnBankerStick_Click(object sender, EventArgs e)
        {
            //calculate best score for banker
            if (bankerAcePresent && (bankerHandScore < 12)) bankerHandScore += 10;
            lblBankerHandScore.Text = bankerHandScore.ToString();
            btnBankerStick.Enabled = false;
            btnBankerDealOne.Enabled = false;
            btnDeal.Enabled = true;
            handFinished();
        }
    }
}
