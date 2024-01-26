using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

[Serializable]
[PostProcess(typeof(LooksHassusRenderer), PostProcessEvent.BeforeStack, "Custom/LooksHassus")]
public sealed class LooksHassus : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("LooksHassus effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };

    [Range(0f, 100f), Tooltip("Darkness start distance")]
    public FloatParameter depthMin = new FloatParameter { value = 0.5f };

    [Range(0f, 100f), Tooltip("Darkness max effect distance")]
    public FloatParameter depthMax = new FloatParameter { value = 0.5f };

    [Range(0f, 100f), Tooltip("Line width")]
    public FloatParameter lineWidth = new FloatParameter { value = 0.5f };

    [Range(0f, 0.001f), Tooltip("Edge threshold")]
    public FloatParameter edgeThreshold = new FloatParameter { value = 0.5f };

    [Range(0f, 1f), Tooltip("Blend1")]
    public FloatParameter blend1 = new FloatParameter { value = 0.5f };

    [Range(0f, 1f), Tooltip("Blend2")]
    public FloatParameter blend2 = new FloatParameter { value = 0.5f };

    [Range(0f, 1f), Tooltip("Blend3")]
    public FloatParameter blend3 = new FloatParameter { value = 0.5f };

    public ColorParameter colour = new ColorParameter();

    public override bool IsEnabledAndSupported(PostProcessRenderContext context)
    {
        return enabled.value && blend.value > 0f;
    }
}

public sealed class LooksHassusRenderer : PostProcessEffectRenderer<LooksHassus>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/LooksHassus"));
        sheet.properties.SetFloat("_EdgeDepthMin", settings.depthMin);
        sheet.properties.SetFloat("_EdgeDepthMax", settings.depthMax);
        sheet.properties.SetFloat("_LineWidth", settings.lineWidth);
        sheet.properties.SetFloat("_EdgeThreshold", settings.edgeThreshold);

        sheet.properties.SetFloat("_Blend1", settings.blend1);
        sheet.properties.SetFloat("_Blend2", settings.blend2);
        sheet.properties.SetFloat("_Blend3", settings.blend3);

        sheet.properties.SetColor("_Colour", settings.colour);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
