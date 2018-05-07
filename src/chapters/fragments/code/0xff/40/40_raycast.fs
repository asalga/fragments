precision mediump float;

// resource 
// https://andorsaga.wordpress.com/2014/01/22/understanding-raycasting-step-by-step/

#define PI 3.141592658
#define DEG_TO_RAD PI/180.
#define TAU PI*2.

#define FOV 45.0

uniform vec2 u_res;

// TODO:
// - review DDA algorithm
// - add 1 wall
// - use texture for level data
// - add kb input

// Theory:
// Cast 'width' number of rays into the scene
// But since we're working with a frag shader
// we'll have some added benefits of working with Y
// so later on texture mapping will be straightforward


float background(vec2 p){
  return step(0., p.y);
}

void main(){
	vec2 a = vec2(1., u_res.y/u_res.x);

	// Normalize the screen coords
	vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);

	// The FOV determines the angle the rays shoot into the scene.
	// the center ray then will be the player's direction.
	// float FOV = .5;

	vec2 playerPos = vec2(0.);
	vec2 playerDir = vec2(0.,1.);
	vec2 playerRight = vec2(1.,0.);
	// vec2 right = vec2(-forward.y, forward.x);

	// We need to calculate the camera line magnitude
	// so that we can calculate the magnitude of the
	// ray vectors.

	// Calculate the base of the viewing 
	// triangle so that we know how far the rays span

	// andorsaga.files.wordpress.com/2014/01/raycast-distortion-fix.jpg

	float camRatio = (FOV/2. * DEG_TO_RAD);

	// Which slice we are working with?
	float sliceXIdx = gl_FragCoord.x;
	
	// float currRaySweep = vec2(p.x);//(2.*sliceXIdx/u_res.x)-1.;

	// Right vector is scaled depending on base of
	// triangle viewing area.

	// Sweep across the viewport, scaling the right vector
	// Note, when p=0, the ray is collinear with playerDir
	vec2 rayDir = vec2(playerDir + (playerRight * camRatio) * p);

	// Find the closest edge, if that edge has a cell filled in, it's a wall,
	// otherwise it's not

	// What is the mag from one x edge to the next x edge?
	// The 'steeper' the rayDir, the greater the value
	// magnitude of how much to 'step' x and y each tile
	// For x, note that the base length of the triangle
	// formed from one step to the other will have length = 1

	float dirStepY;

	// We're trying to get the length of the ray from one x edge
	// to the next
	// We know the x distance is 1
	// Since our ray is normalized, the x will be less than 1 meaning
	// we can 'stretch' it so it reaches the next x-edge. Of course,
	// we'll need to do the same for the y to maintain the ray direction

	float magBetweenEdgeX = length(rayDir * (1./rayDir.x));
	float magBetweenEdgeY = length(rayDir * (1./rayDir.y));






	// Run the DDA
	// ...
	



	float background = background(p);

	float i =   0.0 +
				background +  
				0.0;

	gl_FragColor = vec4(vec3(i), 1.);
}