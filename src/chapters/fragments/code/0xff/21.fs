precision mediump float;
uniform vec2 u_res;

// float horizLineSDF(vec2 p, float w){
//   vec2 a = abs(p);
//   return length(vec2(max(a.x-w,0.), a.y));
// }
// float vertLineSDF(vec2 p, float w){
//   vec2 a = abs(p);
//   return length(vec2(max(a.y-w,0.), a.x));
// }

float roundRectSDF(vec2 p, vec2 d, float r){
	return 1.;
}

void main(){
	vec2 a = vec2(1., u_res.y/u_res.x);
	vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
	float i = 1.;
// 	float i;
// float head = 
// step(vertLineSDF(
// 	p+vec2(0.,-.6), 0.2), 0.47);

// 	float body = step(horizLineSDF(p, 0.2), 0.4);
// 	// float negbody = step(horizLineSDF(p-vec2(0.3, -0.3), 0.2), 0.4);
	
// 	float inter = head * body;
// 	// body -= negbody;
	// body*= head;

// i = inter;
	// i = body;
	// i = head;
// i = head * body;
	   

	gl_FragColor = vec4(vec3(i),1.);
}