struct LightingProperties {
    vec3 ambientStrength;
    vec3 diffuseStrength;
    vec3 specularStrength;
    float shininess;
};

struct DirectionalLight {
    vec3 direction;
    vec3 color;
};

layout(binding = 1) uniform sampler3D skyLightTex;
layout(binding = 2) uniform sampler3D blockLightTex;

uniform int lightMapWidth;
uniform int lightMapHeight;

uniform LightingProperties props;
uniform DirectionalLight directionalLight;

vec3 CalculateDirectionLight(ivec3 pos, vec4 texSample, vec3 normal, vec3 viewDir) {
    vec3 lightDir = normalize(-directionalLight.direction);
    
    float diff = max(dot(normal, lightDir), 0.0);

    vec3 reflectDir = reflect(-lightDir, normal);
    vec3 halfDir = normalize(lightDir + viewDir);
    float spec = pow(max(dot(normal, halfDir), 0.0), props.shininess);
    
    vec3 ambient = props.ambientStrength * texSample.rgb;
    vec3 diffuse = props.diffuseStrength * diff * texSample.rgb;
    vec3 specular = props.specularStrength * spec * texSample.rgb;
    
    float skyLight = ((texelFetch(skyLightTex, pos, 0).r * 255) + 1) / 16;
    float blockLight = ((texelFetch(blockLightTex, pos, 0).r * 255) + 1) / 16;
    
    float lightModifier = max(skyLight, blockLight);
    
    return (ambient + diffuse + specular) * lightModifier;
}