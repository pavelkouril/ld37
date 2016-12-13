using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CommandBufferBlurRefraction : MonoBehaviour
{
	public Shader m_BlurShader;

	private Camera m_Cam;

	private Dictionary<Camera,CommandBuffer> m_Cameras = new Dictionary<Camera,CommandBuffer>();

	private void Cleanup()
	{
        // Clean command buffers
		foreach (var cam in m_Cameras)
		{
			if (cam.Key)
			{
				cam.Key.RemoveCommandBuffer(CameraEvent.AfterSkybox, cam.Value);
			}
		}
		m_Cameras.Clear();
	}

	public void OnEnable()
	{
		Cleanup();
	}

	public void OnDisable()
	{
		Cleanup();
	}

	public void OnWillRenderObject()
	{
		var act = gameObject.activeInHierarchy && enabled;
		if (!act)
		{
			Cleanup();
			return;
		}
		
		var cam = Camera.current;
		if (!cam)
			return;

		CommandBuffer buf = null;
		if (m_Cameras.ContainsKey(cam))
			return;

        // Command buffer just copies screen buffer and stores in texture
		buf = new CommandBuffer();
		buf.name = "Grab screen and blur";
		m_Cameras[cam] = buf;

		int screenCopyID = Shader.PropertyToID("_ScreenCopyTexture");
		buf.GetTemporaryRT (screenCopyID, -1, -1, 0, FilterMode.Point);
		buf.Blit (BuiltinRenderTextureType.CurrentActive, screenCopyID);
		
        buf.SetGlobalTexture("_GrabBlurTexture", screenCopyID);

		cam.AddCommandBuffer (CameraEvent.AfterSkybox, buf);
	}	
}
