#version 460 core

#include "Common/Lighting.glsl"
#include "Common/Wireframe.glsl"

out vec4 FragColor;

layout(location = 0) flat in int atlas;
layout(location = 1) in vec2 texCoords;
layout(location = 2) in vec3 Normal;
layout(location = 3) in vec3 FragPos;
layout(location = 4) in vec3 tangentLightDir;
layout(location = 5) in vec3 tangentViewPos;
layout(location = 6) in vec3 tangentFragPos;
layout(location = 7) in flat ivec3 lightMapPos;

uniform vec4 tint;
layout(binding = 0) uniform sampler2DArray textureSampler;
uniform vec3 viewPos;

void main() { 
    vec2 uv = texCoords;
    if(uv.x > 1.0 || uv.y > 1.0 || uv.x < 0.0 || uv.y < 0.0)
        discard;
    
    vec4 color = texture(textureSampler, vec3(uv, atlas)) * vec4(tint.rgb, 1);
    if (color.a == 0)
        discard;
    
    vec3 normal = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);
    
    color.rgb = CalculateDirectionLight(lightMapPos, color, normal, viewDir);
    
    FragColor = color;
}
