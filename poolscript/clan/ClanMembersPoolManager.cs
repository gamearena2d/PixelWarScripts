using System.Collections.Generic;
using UnityEngine;

public class ClanMembersPoolManager : MonoBehaviour
{
    [Header("📦 Pool Settings")]
    [SerializeField] private GameObject memberPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private int poolSize = 30;

    private readonly List<MemberEntryUI> pool = new();

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(memberPrefab, contentParent);
            obj.SetActive(false);
            pool.Add(obj.GetComponent<MemberEntryUI>());
        }
    }

    public void PopulateMembers(List<ClanMemberData> members)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (i < members.Count)
            {
                pool[i].gameObject.SetActive(true);
                pool[i].Setup(members[i]);
            }
            else
            {
                pool[i].gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        // ESEMPIO DI UTILIZZO AUTOMATICO
        if (ClanManager.Instance != null && ClanManager.Instance.MyClan != null)
        {
            PopulateMembers(ClanManager.Instance.MyClan.Members);
        }
    }
}
