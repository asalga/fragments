// Raycasting! woo!

precision mediump float;
uniform vec2 u_res;
uniform float u_time;

// Define player position
// Define player orientation
// Define walls
// cast 'width' number of rays into the scene
//

float background(vec2 p){
  return step(0., p.y);
}

void main(){
	vec2 a = vec2(1., u_res.y/u_res.x);
	vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
	
	float background = background(p);

	float i =   0.0 +
				background +  
				0.0;

	gl_FragColor = vec4(vec3(i), 1.);
}