// Simple RayCasting
precision mediump float;

#define PI 3.141592658
#define DEG_TO_RAD PI/180.
#define TAU PI*2.

#define FOV 45.
#define HALF_FOV FOV/2.

uniform vec2 u_res;

// TODO:
// - review theory
// - review DDA algorithm
// - add 1 wall
// - use texture for level data
// - add kb input
// define player fov

// Theory:
// Cast 'width' number of rays into the scene
// But since we're working with a frag shader
// we'll have some added benefits of working with Y
// so later on texture mapping will be straightforward
//

// For each fragment, what is our 'x' scanline?

// Q?
// How to get closest edge?


float background(vec2 p){
  return step(0., p.y);
}

void main(){
	vec2 a = vec2(1., u_res.y/u_res.x);

	// Normalize the screen coords
	vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);

	// The FOV determines the angle the rays shoot into the scene.
	// the center ray then will be the player's direction.
	float FOV = 0.5;

	vec2 playerDir = vec2(0.,1.);
	vec2 playerRight = vec2(1., 0.);
	// vec2 right = vec2(-forward.y, forward.x);

	// We need to calculate the camera line magnitude
	// so that we can calculate the magnitude of the
	// ray vectors.

	// Calculate the base of the viewing 
	// triangle so that we know how far the rays span

	// andorsaga.files.wordpress.com/2014/01/raycast-distortion-fix.jpg

	float camRatio = (HALF_FOV * DEG_TO_RAD);

	// Which slice we are working with?
	float sliceXIdx = gl_FragCoord.x;
	
	// float currRaySweep = vec2(p.x);//(2.*sliceXIdx/u_res.x)-1.;

	// Right vector is scaled depending on base of
	// triangle viewing area.

	// Sweep across the viewport, scaling the right vector
	vec2 rayDir = vec2(playerDir + (playerRight * camRatio) * p);





	
	float background = background(p);

	float i =   0.0 +
				background +  
				0.0;

	gl_FragColor = vec4(vec3(i), 1.);
}