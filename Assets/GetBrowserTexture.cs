using UnityEngine;
using System.Collections;

public class GetBrowserTexture : MonoBehaviour {

    public OffscreenCEF BrowserTextureSrc;

    private Material mMtl;

	// Use this for initialization
	void Start ()
    {
        mMtl = GetComponent<MeshRenderer>().material;
        mMtl.SetTexture("_MainTex", BrowserTextureSrc.BrowserTexture);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
