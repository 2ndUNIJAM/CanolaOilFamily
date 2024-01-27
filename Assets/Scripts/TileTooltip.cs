using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TileTooltip : MonoBehaviour
{
    private TMPro.TMP_Text _text;

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
}
