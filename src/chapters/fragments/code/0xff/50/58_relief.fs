// Created by inigo quilez - iq/2013
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
precision mediump float;

float sampleCheckerboard(vec2 p){
  vec2 a = vec2(step(p, vec2(.5)));
  return a.x == 0. && a.y == 0. ? 1. : a.x * a.y;
}

uniform vec2 u_res;
uniform float u_time;

void main(){
  vec2 p = (-u_res.xy + 2.0*gl_FragCoord.xy)/u_res.y;

  p *= 0.75;
  
  float a = atan( p.y, p.x );
  float r = sqrt( dot(p,p) );
  
  a += sin(0.5*r-0.5*iTime );
	
  float h = 0.5 + 0.5*cos(9.0*a);

  float s = smoothstep(0.4,0.5,h);

  vec2 uv;
  uv.x = iTime + 1.0/(r + .1*s);
  uv.y = 3.0*a/3.1416;

  vec3 col = vec3(sampleCheckerboard(uv))
  //texture( iChannel0, uv ).xyz;

  float ao = smoothstep(0.0,0.3,h)-smoothstep(0.5,1.0,h);
  col *= 1.0 - 0.6*ao*r;
	col *= r;

  fragColor = vec4( col, 1.0 );
}