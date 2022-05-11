# SharpRID
SharpRID, Automated Windows Guest Account RID Hijacking tools for Windows Client. To perform the attack, it will be sufficient to run the tool with system privileges.


![Attack With Local Admin Privileges](https://github.com/taftss/SharpRID/blob/master/Screenshot_40.png)


## Usage
To perform the attack, it will be sufficient to run the tool with system privileges. Because the SAM values on the registry need to be changed. Therefore, it should be run with system privileges. If you are in the "Local Admin" group, you can run it with PsExec64.exe (sysinternals).

To perform an attack with local admin privileges;

```
PsExec64.exe -s -i C:\Users\taftss\Desktop\RidHijacking.exe
```

To perform an attack with system privileges;

```
C:\Users\taftss\Desktop\RidHijacking.exe
```


![Attack With Local Admin Privileges](https://raw.githubusercontent.com/taftss/SharpRID/main/SharpRID%20PoC.gif?token=GHSAT0AAAAAABQ4OOACRPTUS4H3FDEMIYVMYT3SGLQ)


### What is Windows RID?

Windows operating systems use the RID (Relative Identifier) to differentiate groups and user accounts. It is part of the Security Identifier (SID) and every time a new account or a group is created the number is increased by one. The local administrator group RID is always 500 and standard users or groups typically start with the number 1001. 

### What does RID do?

Red team operators and penetration testers can increase the privileges of user accounts they have created by changing their RID values. In addition, users whose privileges have been increased as a result of RID Hijacking will not appear in the "Local Administrators" group.

### What is RID Hijacking?

RID Hijacking is usually done through the Windows guest account. Because RID Hijacking aims to be persistence. Access to guest accounts is usually open. With the guest account whose privileges have been increased, it will be persistence.

For this purpose, the RID value of the guest account is manipulated and the result is obtained.
