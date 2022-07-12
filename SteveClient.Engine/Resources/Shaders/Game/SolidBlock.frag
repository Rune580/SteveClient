#version 460 core

out vec4 FragColor;

layout(location = 0) in vec2 texCoord;
layout(location = 1) flat in float atlas;

uniform vec4 tint;
uniform sampler2DArray textureSampler;

void main() {
    vec4 color = texture(textureSampler, vec3(texCoord, atlas)) * vec4(tint.rgb, 1);
    
    if (color.a == 0)
        discard;
    
    FragColor = color;
}
