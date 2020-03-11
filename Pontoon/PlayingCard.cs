using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pontoon
{
    class PlayingCard
    {
        public int indexNo; // 0..12 Club Ace - King, 13-25 Diamonds, 26-38 Hearts, 39-51 Spades
        public string display; // 'Ace', '2', '3' .. '10', "Jack", "Queen", "King"
        public int suit; // 0=Club, 1=Diamond, 2= Heart, 3=Spade
        public int listItem; // 1 = Ace, 2= "2".. 10 = "10", 11 = "Jack", 12 = "Queen", 13 = "King"
        public int value; // Ace is 1, "2"= 2.. "10" is 10, "Jack" is 10, "Queen" is 10, "King" is 10

        // numeric value, need to create exception for Ace

        public PlayingCard(int indexNo)
        {
            this.indexNo = indexNo;
            this.suit = (int)(indexNo / 13);
            this.listItem = (int)(indexNo % 13)+1;
            this.value = (this.listItem<11)?this.listItem:10; // force 11,12,13 to be valued at 10
            switch (this.listItem)
            {
                case 1:
                    this.display = "Ace";
                    break;
                case 11:
                    this.display = "Jack";
                    break;
                case 12:
                    this.display = "Queen";
                    break;
                case 13:
                    this.display = "King";
                    break;
                default:
                    this.display = this.listItem.ToString(); // simple numeric
                    break;
            }
        }
    }
}
