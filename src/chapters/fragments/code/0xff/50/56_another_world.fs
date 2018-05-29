precision mediump float;

uniform vec2 u_res;
uniform float u_time;

// Based on https://www.shadertoy.com/view/4sSSzG
float t (vec2 st,
                vec2 p0, vec2 p1, vec2 p2
                ){
  vec3 e0, e1, e2;
  p0 /= vec2(700.);
  p1 /= 700.;
  p2 /= 700.;
  float smoothness = 0.001;

  e0.xy = normalize(p1 - p0).yx * vec2(+1.0, -1.0);
  e1.xy = normalize(p2 - p1).yx * vec2(+1.0, -1.0);
  e2.xy = normalize(p0 - p2).yx * vec2(+1.0, -1.0);

  e0.z = dot(e0.xy, p0) - smoothness;
  e1.z = dot(e1.xy, p1) - smoothness;
  e2.z = dot(e2.xy, p2) - smoothness;

  float a = max(0.0, dot(e0.xy, st) - e0.z);
  float b = max(0.0, dot(e1.xy, st) - e1.z);
  float c = max(0.0, dot(e2.xy, st) - e2.z);

  return smoothstep(smoothness * 2.0,
                    1e-7,
                    length(vec3(a, b, c)));
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  // vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  vec2 p = gl_FragCoord.xy/u_res;
  float i;
  float b;
  
  p.y = 1.-p.y;
  
  // Shirt
  b +=  t(p,vec2(0.,556.),
            vec2(147., 516.),
            vec2(.0, 600.));

  b +=  t(p,vec2(.0, 600.),
            vec2(400., 700.),
            vec2(0.0, 700.));


  b +=  t(p,vec2(0., 580.),
            vec2(670., 600.),
            vec2(0.0, 700.));

  b +=  t(p,vec2(670., 600.),
            vec2(770., 800.),
            vec2(0.0, 700.));
  b +=  t(p,vec2(400., 575.),
            vec2(469., 544.),
            vec2(669.0, 600.));
  ///
  b +=  t(p,vec2(400., 575.),
            vec2(666., 599.),
            vec2(340.0, 700.));
  b +=  t(p,vec2(.0, 600.),
            vec2(40., 550.),
            vec2(400.0, 700.));

  b +=  t(p,vec2(0.,556.),
            vec2(122., 532.),
            vec2(450., 670.));
  b +=  t(p,vec2(170.,583.),
            vec2(400., 577.),
            vec2(450., 670.));

  b +=  t(p,vec2(400.,577.),
            vec2(430., 545.),
            vec2(468., 545.));
    b +=  t(p,vec2(468.,545.),
            vec2(473., 534.),
            vec2(670., 600.));
  // Neck


  // Face


  // Hair



  gl_FragColor = vec4(vec3(b),1.);
}