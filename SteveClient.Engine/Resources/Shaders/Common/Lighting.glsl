layout(std430, binding = 0) buffer chunkLightData {
    int skyLights[4096];
    int blockLights[4096];
};

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

uniform LightingProperties props;
uniform DirectionalLight directionalLight;

vec3 CalculateDirectionLight(int blockPos, vec4 texSample, vec3 normal, vec3 viewDir) {
    vec3 lightDir = normalize(-directionalLight.direction);
    
    float diff = max(dot(normal, lightDir), 0.0);

    vec3 reflectDir = reflect(-lightDir, normal);
    vec3 halfDir = normalize(lightDir + viewDir);
    float spec = pow(max(dot(normal, halfDir), 0.0), props.shininess);
    
    vec3 ambient = props.ambientStrength * texSample.rgb;
    vec3 diffuse = props.diffuseStrength * diff * texSample.rgb;
    vec3 specular = props.specularStrength * spec * texSample.rgb;
    
    int skyLight = skyLights[blockPos];
    int blockLight = blockLights[blockPos];
    
    float lightModifier = (1 + max(skyLight, blockLight)) / 16.0;
    
    return (ambient + diffuse + specular) * lightModifier;
}