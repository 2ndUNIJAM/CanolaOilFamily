using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TileTooltip : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text _headText;
    [SerializeField]
    private TMPro.TMP_Text _text;

    private string _head;
    public string Head
    {
        get { return _head; }
        set
        {
            _head = value;
            _headText.text = _head;
        }
    }

    private string _desc;
    public string Description
    {
        get { return _desc; }
        set 
        { 
            _desc = value;
            _text.text = _desc;
        }
    }

    public void SetPosition(Vector3 position, int r)
    {
        if (r <= -1)
            transform.position = position - new Vector3(0, 1.2f);
        else
            transform.position = position + new Vector3(0, 1.2f);
    }

    public void SetText(string h, string d)
    {
        Head = h; Description = d;
    }
}
