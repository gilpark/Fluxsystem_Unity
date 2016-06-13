# Fluxsystem_Unity

This is a simple Vectorfield(aka flowfield) for Unity.

![alt text](http://gilpark.com/wp-content/uploads/2016/06/fluxsystem0.png "Fluxsystem_Unity")

It is still WIP and supports only 2D, which means the object's only **X** and **Z** positions are updated on the field.
3D Vectorfield will soon be implemented.
More information about [vectorfield / goal based path finding](http://gamedevelopment.tutsplus.com/tutorials/understanding-goal-based-vector-field-pathfinding--gamedev-9007)

The system consists of 3 parts:
* ***Fluxsystem class*** is the main class that initiates & calculate the field.
* ***Cell class*** is holding infomation of its & neighbors' location in the array.
* ***flow class*** is for looking up vector in the cell in the field and updates object's position.

[![IMAGE ALT TEXT HERE](http://img.youtube.com/vi/7XekR9gT9Rk/0.jpg)](http://www.youtube.com/watch?v=7XekR9gT9Rk)

