precision mediump float;

// resources
// andorsaga.wordpress.com/2014/01/22/understanding-raycasting-step-by-step/
// en.wikipedia.org/wiki/Digital_differential_analyzer_%28graphics_algorithm%29	
// andorsaga.files.wordpress.com/2014/01/raycast-distortion-fix.jpg

// Cast 'width' number of rays into the scene
// But since we're working with a frag shader
// we'll have some added benefits of working with Y
// so later on texture mapping will be straightforward

#define PI 3.141592658
#define DEG_TO_RAD PI/180.
#define TAU PI*2.
#define FOV 45.0
#define MAX_STEPS 10.
#define ONE 1.

#define NORM(v) (v*2.-1.)

uniform vec2 u_res;

float background(vec2 p){
  return step(0., p.y);
}

void main(){
	vec2 a = vec2(1., u_res.y/u_res.x);

	// Normalize the screen coords
	vec2 p = a*( NORM(gl_FragCoord.xy/u_res));

	// The FOV determines the angle the rays shoot into the scene.
	// the center ray then will be the player's direction.
	// float FOV = .5;

	vec2 playerPos = vec2(ZERO);
	vec2 playerDir = vec2(ZERO,ONE);
	vec2 playerRight = vec2(-playerDir.y, playerDir.x);

	// We need to calculate the camera line magnitude
	// so that we can calculate the magnitude of the ray vectors.

	// Calculate the base of the viewing 
	// triangle so that we know how far the rays span

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

	// We're trying to get the length of the ray from one x edge
	// to the next.  We know the x distance is 1.
	// Since our ray is normalized, the x will be less than 1 meaning
	// we can 'stretch' it so it reaches the next x-edge. Of course,
	// we'll need to do the same for the y to maintain the ray direction.
	// - Also you can think of this as the diagonal length between edges
	// float magBetweenEdgeX = length(rayDir * (1./rayDir.x));
	// float magBetweenEdgeY = length(rayDir * (1./rayDir.y));

	vec2 magBetweenEdges = vec2(length(rayDir * (1./rayDir.x)),
								length(rayDir * (1./rayDir.y)));

	vec2 magToEdge = (vec2(ONE) - playerPos) * magBetweenEdges;

	// Calculating Starting Mag/Offset
	// components will be -1 or 1
	vec2 dirStep = NORM(step(0., rayDir));

	// Run the DDA (Digital Differential Analyzer)

	// X and Y values 'race'. We compare each and increase the lesser value.
	// Loop terminates on reaching a non-empty cell.
	// We're in GLSL so we require a constant expression 
	// in the comparison part of our loop--just break out early.
	for(float s = 0.; s < MAX_STEPS; s+=1.){

	}

	float i =   0.0 +
				background(p) +
				0.0;

	gl_FragColor = vec4(vec3(i), 1.);
}