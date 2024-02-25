using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[System.Serializable]
public class PackageInfomation
{
    [Title("Package")]
    public PackageType packageType = PackageType.Default;

    [Title("Reward")]
    public List<ReceiveInformation> receiveInformationList = new List<ReceiveInformation>();
}


[CreateAssetMenu(fileName = "PackageDataBase", menuName = "ScriptableObjects/PackageDataBase")]
public class PackageDataBase : ScriptableObject
{
    public List<PackageInfomation> PackageInfomationList = new List<PackageInfomation>();

    public PackageInfomation GetPackageInfomation(PackageType type)
    {
        PackageInfomation package = new PackageInfomation();

        for (int i = 0; i < PackageInfomationList.Count; i++)
        {
            if (PackageInfomationList[i].packageType.Equals(type))
            {
                package = PackageInfomationList[i];
                break;
            }
        }

        return package;
    }
}
