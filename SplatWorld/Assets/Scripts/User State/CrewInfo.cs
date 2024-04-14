using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewInfo 
{
    // these are filled out by json utility
    private int id;
    private string name;
    private string joincode;
    private int owner;
    private byte[] color_r;
    private byte[] color_g;
    private byte[] color_b;

    // these are not
    private UserInfo ownerInfo;
    private Color color;
    private List<UserInfo> members;
    private DynamicGetRequest requester;

    public void GenerateColor()
    {
        float red = BitConverter.ToSingle(color_r, 7);
        float green = BitConverter.ToSingle(color_g, 7);
        float blue = BitConverter.ToSingle(color_b, 7);
        color = new Color(red, green, blue);
    }

    public void GenerateOwnerInfo()
    {
        requester = new DynamicGetRequest("https://splatworld.alchemi.dev/get-user", $"&id={owner}");
        requester.GetData();
    }

    public string GetCrewMembersLink()
    {
        return $"https://splatworld.alchemi.dev/get-crew-members";
    }

    public override string ToString()
    {
        return $"Crew Name: {name} Joincode: {joincode} ID: {id} Owner ID: {owner} Color_R: {color_r} Color_G: {color_g} Color_B: {color_b}";
    }


}
