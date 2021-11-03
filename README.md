# Player Depth Chart
A sample ASP NET Core API to insert, delete and get players to different position list.

<h3>Assumptions</h3>
<ul>
 <li>No database is used to store the list. A static dictionary is used to achieve this</li>
 <li>We assume position depth is provided as the next available position depth or existing position depth. If there are depths 0 and 1 in the list, you cannot add to position 3 before filling position 2. You may add the 0, 1 or 2.</li>
 <li>No negative position depths are implemented.</li>
 <li>Though the logic behind MLB and NFL are the same, NFL is implemented separateley to demonstrate the capability to have it's own implementation. MLB is inherited from the base class</li>

</ul>

<h3>Running the project</h3>
This project implements swagger endpoints to test.
<ul>
<li>Parameters include a Game type like NFL and MLB. This is to make the endpoints generic instead of having separate for each type. The game type is extracted from the url like "https://localhost:44332/api/PlayerDepth/NFL/add" gives NFL as the game type.</li>
<li></li>
</ul>  

<h3>Possible improvements</h3>

<ul>
<li>Methods can be made async</li>
<li>Tests can be made generic since the logic is same for NFL and MLB</li>
<li>Reflection can be used in the factory to get the assignable classes</li>
</ul
