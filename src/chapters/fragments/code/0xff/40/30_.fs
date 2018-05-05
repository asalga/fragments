precision mediump float;

#define PI 3.141592658
#define TAU PI*2.

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
	vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);

	// The FOV determines the angle the rays shoot into the scene.
	float FOV = 0.5;

	// Get current x

	

	// Calculate the base of the viewing 
	// triangle so that we know how far the rays span



	// 
	float camPlaneMag = (FOV/2.)




	vec2 forward = vec2(0., 1.);
	vec2 right = vec2(-forward.y, forward.x);

	
	float background = background(p);

	float i =   0.0 +
				background +  
				0.0;

	gl_FragColor = vec4(vec3(i), 1.);
}