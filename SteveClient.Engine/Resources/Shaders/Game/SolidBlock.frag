#version 330 core

out vec4 FragColor;

in vec2 texCoord;

uniform vec4 tint;
uniform sampler2D textureSampler;

void main() {
    vec4 color = texture(textureSampler, texCoord) * vec4(tint.rgb, 1);
    
    if (color.a == 0)
        discard;
    
    FragColor = color;
}
