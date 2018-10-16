precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float PI = 3.141592658;

float sdCircle(in vec2 p, in float r){
  return length(p)-r;
}

void main(void){
    vec2 a  = vec2(1., u_res.y/u_res.x);
    vec2 p = a*((gl_FragCoord.xy/u_res.xy)*2.-1.);

    p*=1.5;
    p.x += sin(p.y*15. + u_time*10.)/10. * sin(u_time*5.);
    p.y += sin(p.x*15. + u_time*10.)/10. * sin(u_time*5.);


    float i = step(sdCircle(p, 0.85), 0.);

    i *= step(sin(length(p)*120.), 0.);
    // i -= step(sdCircle(p, 0.75), 0.);

    gl_FragColor = vec4(vec3(i),1);
}


