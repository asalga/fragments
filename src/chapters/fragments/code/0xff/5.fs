precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.14159
#define CNT 10
vec2 test[CNT];

void main(){
	vec2 a = vec2(1., u_res.y/u_res.x);
	vec2 p = a * (gl_FragCoord.xy /u_res * 2. -1.);
	float i = 0.;
	for(int e = 0; e < CNT; e++){
		float theta = float(e) * (2.*PI)/float(CNT);
		test[e] = vec2(
						cos(theta)*.5 * (0.95 + sin(u_time*10.)*.043), 
						sin(theta)*.5 * (0.95 + cos(u_time*10.)*.043)
				);
		float dist = .021/ distance(p, test[e]);
		i += pow(dist, 1.);
	}
	float d = .18 / distance(p, vec2(0.));
	i += pow(d, 1.);
	i = smoothstep(0.99, 1., i);
	i -= 1. - smoothstep(0.2,0.21, length(p-vec2(0.)));

	gl_FragColor = vec4(vec3(i), 1.);
}