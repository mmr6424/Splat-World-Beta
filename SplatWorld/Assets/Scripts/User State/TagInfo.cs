using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagInfo 
{
    private int id;
    private Image image;
    private Image thumbnail;
    private string json;
    private string title;
    
    private DateTime createDate;
    private bool active;
    private bool saved;
    private int points;
    
    private bool flagged;
    private double latitude;
    private double longitude;

    private CrewInfo crewAffiliation;
    private UserInfo author;
}
