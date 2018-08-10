// 131
precision mediump float;

uniform vec2 u_res;
uniform vec3 u_mouse;
uniform  float u_time;

#define CNT 40

float random (vec2 st) {
  return fract(sin(dot(st.xy,vec2(12.9898,78.233)))*43758.5453123);
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  vec3 c;
  vec2 points[CNT];

  // populate with values
  for(int i = 0; i < CNT; i++){
    points[i] = vec2( 2.*fract(cos(float(i)*1000.0))-1.,
                      2.*fract(sin(float(i)*20000.0))-1.);
  }

  float dist = length(p - points[0]);

  for(int i = 1; i < CNT; i++){
    float testLength = length(p-points[i]);
    if(testLength < dist){
      dist = testLength;
      c = vec3(dist);
    }
  }

  gl_FragColor = vec4(vec3(c),1);
}