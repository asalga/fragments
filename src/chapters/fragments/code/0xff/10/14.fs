 
 
 
 
 
 
 
 
                         precision
                        lowp float;
                        uniform vec2 
                        u_res; float          
              rect( vec2 p){vec2 a=abs(p*1.00);
              return max(a.x, a.y);}void main()
              {vec2 as=vec2(1.00305010,u_res.y/
              u_res.x);;;;;vec2 p=gl_FragCoord.
                        xy/ u_res.xy*           
                        2.0-1.; float
                        i=step( rect(
                        p * vec2( 1.0
                        ,.15)), 0.25)
                        ;i+=step(rect
                        (vec2(0.,-0.4
                        )+p * vec2(.1
                        ,1.001)),.15)
                        ;gl_FragColor
                        =vec4(vec3(i)
                        ,1.);}//cross