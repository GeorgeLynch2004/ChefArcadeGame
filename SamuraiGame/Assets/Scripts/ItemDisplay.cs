using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private TMP_Text m_NameOfItem;

    public void SetNameOfItem(string name)
    {
        m_NameOfItem.text = name;
    }

    public string GetNameOfItem()
    {
        return m_NameOfItem.text;
    }

    public void SetImageOfItem(Sprite sprite)
    {
        m_SpriteRenderer.sprite = sprite;
    }

    public Sprite GetImageOfItem()
    {
        return m_SpriteRenderer.sprite;
    }

    public void ClearDisplay()
    {
        m_SpriteRenderer.sprite = null;
        m_NameOfItem.text = null;
    }


}
