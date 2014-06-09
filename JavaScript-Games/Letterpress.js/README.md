Letterpress.js by Team Dairou
=============================
A JavaScript implementation of the word game Letterpress for iOS. 

Given a 25-letter square board, players take turns discovering words. The letters of that word then change to the player’s color (red versus blue). When the board is completely colored, the player with the most colored letters wins. The twist: unless you completely surround a letter (on all 4 sides), your opponent can steal it back.

### Game Rules

1. Using an opponent's tile gives you one point and takes away one point from your opponent. These tiles will also change in color to your side's color.
2. If a colored tile is surrounded on all 4 sides by tiles of similar color, it will become a darker color. If you use this tile, you will receive no points.
3. When dark colored tiles are used, they remain a dark color (they do not change to your side's color).
4. If you use letter tiles from around a darker colored tile, it will return it to a lighter color.
5. The game ends when all the tiles are used. The winner is the player with more points than the other.

---
Letterpress is a trademark of [atebits](http://www.atebits.com/letterpress/).
