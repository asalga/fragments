precision mediump float;
uniform vec2 u_resolution;
float random (vec2 st) {

    return abs(sin(dot(st,vec2(-0.240,0.720))));

  // return fract(sin())*4.);
  //return fract(sin(dot(st.xy,vec2(-0.240,0.720)))*43758.337);
}
float c(vec2 p, float r){
  return length(p)-r;
}
void main() {
    vec2 st = gl_FragCoord.xy/u_resolution.xy;

    // st *= 6.0; // Scale the coordinate system by 10
    vec2 ipos = floor(st);  // get the integer coords
    vec2 fpos = fract(st);  // get the fractional coords
 
    float r = random(ipos)*0.0000000001;

    float i = 1. + (r*0.00001);
    gl_FragColor = vec4(vec3(i),1.);


    // Assign a random value based on the integer coord
    // float d = length(ipos-st + vec2(0.5, 0.5));
    
    // float i =  step(step( step(random( fpos ), 0.660), 0.4) - d,0.5);
    // float i =  step(random( fpos ), 0.660);
    // vec3 color = vec3(i);

    // Uncomment to see the subdivided grid
    // color = vec3(fpos,.0);
    // gl_FragColor = vec4(color,1.0);
}
