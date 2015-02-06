RTS camera for Unity
====================

Attach this script to a camera and then you can use it RTS-style.

![](http://evanhahn.com/tape/unity_rts.gif)

*Note: I'm a Unity newbie, so I'm sorry if I don't know what I'm doing! Happy to accept pull requests.*

## Quick start

1. Attach this script to a camera.
2. Add "Mouse Look" and "Mouse Select" to your inputs. I recommend changing the default Fire1 and Fire2 for left and right mouse buttons.
3. Make sure selectable objects have 3D colliders on them.
4. For any selectable object, add the following code:

   ```
   void OnTriggerEnter(Collider other) {
     if (other.gameObject.name == "RTS Selection") {
       // This object has been selected; do stuff!
     }
   }

   void OnTriggerExit(Collider other) {
     if (other.gameObject.name == "RTS Selection") {
       // This object has been deselected; do stuff!
     }
   }
   ```

That's it!

For more usage, check out the demo.

## Class RTSCamera

One RTSCamera should be attached to the main camera for your game.

<table>

<tr>
<th>Property</th>
<th>Type</th>
<th>Default</th>
<th>Description</th>
</tr>

<tr>
<td><code>disablePanning</code></td>
<td><code>bool</code></td>
<td><code>false</code></td>
<td>When true, the player cannot pan the camera.</td>
</tr>

<tr>
<td><code>disableSelect</code></td>
<td><code>bool</code></td>
<td><code>false</code></td>
<td>When true, the player cannot select.</td>
</tr>

<tr>
<td><code>disableZoom</code></td>
<td><code>bool</code></td>
<td><code>false</code></td>
<td>When true, the player cannot zoom.</td>
</tr>

<tr>
<td><code>selectColor</code></td>
<td><code>Color</code></td>
<td><code>Color.green</code></td>
<td>The selection box drawn on-screen will be this color.</td>
</tr>

<tr>
<td><code>selectLineWidth</code></td>
<td><code>float</code></td>
<td><code>2f</code></td>
<td>The selection box drawn on-screen will have this line width.</td>
</tr>

<tr>
<td><code>maximumZoom</code></td>
<td><code>float</code></td>
<td><code>1f</code></td>
<td>Maximum zoom; minimum camera scale. (This value will be less than <code>minimumZoom</code>, which might seem backwards.)</td>
</tr>

<tr>
<td><code>minimumZoom</code></td>
<td><code>float</code></td>
<td><code>20f</code></td>
<td>Minimum zoom; maximum camera scale. (This value will be greater than <code>maximumZoom</code>, which might seem backwards.)</td>
</tr>

<tr>
<td><code>lookDamper</code></td>
<td><code>float</code></td>
<td><code>5f</code></td>
<td>Panning speed will be divided by this value. A higher number makes for slower panning.</td>
</tr>

<tr>
<td><code>selectionObjectName</code></td>
<td><code>string</code></td>
<td><code>"RTS Selection"</code></td>
<td>When a selection happens, a trigger (a BoxCollider) will be dropped briefly into the scene. It will have this name.</td>
</tr>

</table>

Enjoy!
