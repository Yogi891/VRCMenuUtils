﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using VRC.UI;

using VRCMenuUtils;

namespace VRChat.UI
{
    public static class VRCEUi
    {
        #region VRChat Menu Variables
        private static QuickMenu _quickMenu;
        private static GameObject _screenHeader;

        private static GameObject _avatarScreen;
        private static GameObject _detailsScreen;
        private static GameObject _playlistsScreen;
        private static GameObject _socialScreen;
        private static GameObject _settingsScreen;
        private static GameObject _safetyScreen;
        private static GameObject _userInfoScreen;
        private static GameObject _worldsScreen;
        #endregion
        #region VRChat Menu Properties
        public static QuickMenu QuickMenu
        {
            get
            {
                if (_quickMenu == null)
                    _quickMenu = ((QuickMenu)typeof(QuickMenu).GetMethod("get_Instance", BindingFlags.Public | BindingFlags.Static).Invoke(null, null));
                return _quickMenu;
            }
        }
        public static GameObject ScreenHeader
        {
            get
            {
                if (_screenHeader == null)
                    _screenHeader = GameObject.Find("UserInterface/MenuContent/Backdrop/Header");
                return _screenHeader;
            }
        }

        public static GameObject AvatarScreen
        {
            get
            {
                if(_avatarScreen == null)
                    _avatarScreen = GameObject.Find(QuickMenu.avatarScreenPath);
                return _avatarScreen;
            }
        }
        public static GameObject DetailsScreen
        {
            get
            {
                if (_detailsScreen == null)
                    _detailsScreen = GameObject.Find(QuickMenu.detailsScreenPath);
                return _detailsScreen;
            }
        }
        public static GameObject PlaylistsScreen
        {
            get
            {
                if (_playlistsScreen == null)
                    _playlistsScreen = GameObject.Find(QuickMenu.playlistsScreenPath);
                return _playlistsScreen;
            }
        }
        public static GameObject SocialScreen
        {
            get
            {
                if (_socialScreen == null)
                    _socialScreen = GameObject.Find(QuickMenu.socialScreenPath);
                return _socialScreen;
            }
        }
        public static GameObject SettingsScreen
        {
            get
            {
                if (_settingsScreen == null)
                    _settingsScreen = GameObject.Find(QuickMenu.settingsScreenPath);
                return _settingsScreen;
            }
        }
        public static GameObject SafetyScreen
        {
            get
            {
                if (_safetyScreen == null)
                    _safetyScreen = GameObject.Find(QuickMenu.safetyScreenPath);
                return _safetyScreen;
            }
        }
        public static GameObject UserInfoScreen
        {
            get
            {
                if (_userInfoScreen == null)
                    _userInfoScreen = GameObject.Find(QuickMenu.userInfoScreenPath);
                return _userInfoScreen;
            }
        }
        public static GameObject WorldsScreen
        {
            get
            {
                if (_worldsScreen == null)
                    _worldsScreen = GameObject.Find(QuickMenu.worldsScreenPath);
                return _worldsScreen;
            }
        }
        #endregion
        #region VRChat UI Functions
        public static Transform DuplicateButton(Transform button, string name, string text, Vector2 offset, Transform parent = null)
        {
            // Create new one
            GameObject goButton = GameObject.Instantiate(button.gameObject);
            if(goButton == null)
            {
                MVRCLogger.LogError("Could not duplicate button!");
                return null;
            }

            // Get UI components
            Button objButton = goButton.GetComponentInChildren<Button>();
            Text objText = goButton.GetComponentInChildren<Text>();

            // Destroy broke components
            GameObject.DestroyImmediate(goButton.GetComponent<RectTransform>());

            // Set required parts
            if (parent != null)
                goButton.transform.SetParent(parent);
            goButton.name = name;

            // Modify RectTransform
            RectTransform rtOriginal = button.GetComponent<RectTransform>();
            RectTransform rtNew = goButton.GetComponent<RectTransform>();

            rtNew.localScale = rtOriginal.localScale;
            rtNew.anchoredPosition = rtOriginal.anchoredPosition;
            rtNew.sizeDelta = rtOriginal.sizeDelta;
            rtNew.localPosition = rtOriginal.localPosition + new Vector3(offset.x, offset.y, 0f);
            rtNew.localRotation = rtOriginal.localRotation;

            // Change UI properties
            objText.text = text;
            objButton.onClick = new Button.ButtonClickedEvent();

            // Finish
            return goButton.transform;
        }
        #endregion

        #region VRChat Menu Screen Classes
        public static class InternalQuickMenu
        {
            #region QuickMenu Variables
            private static Transform _shortcutMenu;
            private static Transform _cameraMenu;
            private static Transform _emojiMenu;
            private static Transform _newElements;

            private static FieldInfo _fiCurrentPage;
            #endregion
            #region QuickMenu Properties
            public static Transform ShortcutMenu
            {
                get
                {
                    if(_shortcutMenu == null)
                    {
                        if (QuickMenu == null)
                            return null;
                        _shortcutMenu = QuickMenu.transform.Find("ShortcutMenu");
                    }
                    return _shortcutMenu;
                }
            }
            public static Transform CameraMenu
            {
                get
                {
                    if (_cameraMenu == null)
                    {
                        if (QuickMenu == null)
                            return null;
                        _cameraMenu = QuickMenu.transform.Find("CameraMenu");
                    }
                    return _cameraMenu;
                }
            }
            public static Transform EmojiMenu
            {
                get
                {
                    if (_emojiMenu == null)
                    {
                        if (QuickMenu == null)
                            return null;
                        _emojiMenu = QuickMenu.transform.Find("EmojiMenu");
                    }
                    return _emojiMenu;
                }
            }
            public static Transform NewElements
            {
                get
                {
                    if (_newElements == null)
                    {
                        if (QuickMenu == null)
                            return null;
                        _newElements = QuickMenu.transform.Find("QuickMenu_NewElements");
                    }
                    return _newElements;
                }
            }

            public static GameObject CurrentPage
            {
                get
                {
                    if(_fiCurrentPage == null)
                    {
                        _fiCurrentPage = typeof(QuickMenu).GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                                            .Where(a => a.FieldType == typeof(GameObject))
                                            .LastOrDefault(a => (GameObject)a.GetValue(QuickMenu) == ShortcutMenu.gameObject);
                        if (_fiCurrentPage == null)
                            return null;
                        MVRCLogger.Log("Found QuickMenu currentPage on: " + _fiCurrentPage.Name);
                    }

                    return _fiCurrentPage.GetValue(QuickMenu) as GameObject;
                }
                set
                {
                    if (_fiCurrentPage == null)
                    {
                        _fiCurrentPage = typeof(QuickMenu).GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                                            .Where(a => a.FieldType == typeof(GameObject))
                                            .LastOrDefault(a => (GameObject)a.GetValue(QuickMenu) == ShortcutMenu.gameObject);
                        if (_fiCurrentPage == null)
                            return;
                        MVRCLogger.Log("Found QuickMenu currentPage on: " + _fiCurrentPage.Name);
                    }

                    _fiCurrentPage.SetValue(QuickMenu, value);
                }
            }
            #endregion

            #region ShortcutMenu Variables
            private static Transform _reportWorldButton;
            #endregion
            #region ShortcutMenu Properties
            public static Transform ReportWorldButton
            {
                get
                {
                    if(_reportWorldButton == null)
                    {
                        if (ShortcutMenu == null)
                            return null;
                        _reportWorldButton = ShortcutMenu.Find("ReportWorldButton");
                    }
                    return _reportWorldButton;
                }
            }
            #endregion

            #region NewElements Variables
            private static Transform _infoBar;
            #endregion
            #region NewElements Properties
            public static Transform InfoBar
            {
                get
                {
                    if (_infoBar == null)
                    {
                        if (NewElements == null)
                            return null;
                        _infoBar = NewElements.Find("_InfoBar");
                    }
                    return _infoBar;
                }
            }
            #endregion
        }
        public static class InternalUserInfoScreen
        {
            #region UserInfo Variables
            private static PageUserInfo _instance;
            #endregion
            #region UserInfo Properties
            public static PageUserInfo Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        if (UserInfoScreen == null)
                            return null;
                        _instance = UserInfoScreen.GetComponent<VRCUiPage>() as PageUserInfo;
                    }
                    return _instance;
                }
            }
            #endregion

            #region UserInfo UI Variables
            private static Transform _userPanel;
            private static Transform _avatarImage;
            #endregion
            #region UserInfo UI Properties
            public static Transform UserPanel
            {
                get
                {
                    if (_userPanel == null)
                    {
                        if (UserInfoScreen == null)
                            return null;
                        _userPanel = UserInfoScreen.transform.Find("User Panel");
                    }
                    return _userPanel;
                }
            }
            public static Transform AvatarImage
            {
                get
                {
                    if (_avatarImage == null)
                    {
                        if (UserInfoScreen == null)
                            return null;
                        _avatarImage = UserInfoScreen.transform.Find("AvatarImage");
                    }
                    return _avatarImage;
                }
            }
            #endregion

            #region UserPanel Variables
            private static Transform _moderator;
            private static Transform _user;
            private static Transform _onlineFriend;
            private static Transform _offlineFriend;

            private static Transform _playlistsButton;
            private static Transform _favoriteButton;
            private static Transform _reportButton;
            private static Transform _usernameText;
            #endregion
            #region UserPanel Properties
            public static Transform Moderator
            {
                get
                {
                    if (_moderator == null)
                    {
                        if (UserPanel == null)
                            return null;
                        _moderator = UserPanel.Find("Moderator");
                    }
                    return _moderator;
                }
            }
            public static Transform User
            {
                get
                {
                    if (_user == null)
                    {
                        if (UserPanel == null)
                            return null;
                        _user = UserPanel.Find("User");
                    }
                    return _user;
                }
            }
            public static Transform OnlineFriend
            {
                get
                {
                    if (_onlineFriend == null)
                    {
                        if (UserPanel == null)
                            return null;
                        _onlineFriend = UserPanel.Find("OnlineFriend");
                    }
                    return _onlineFriend;
                }
            }
            public static Transform OfflineFriend
            {
                get
                {
                    if (_offlineFriend == null)
                    {
                        if (UserPanel == null)
                            return null;
                        _offlineFriend = UserPanel.Find("OfflineFriend");
                    }
                    return _offlineFriend;
                }
            }

            public static Transform PlaylistsButton
            {
                get
                {
                    if (_playlistsButton == null)
                    {
                        if (UserPanel == null)
                            return null;
                        _playlistsButton = UserPanel.Find("Playlists");
                    }
                    return _playlistsButton;
                }
            }
            public static Transform FavoriteButton
            {
                get
                {
                    if (_favoriteButton == null)
                    {
                        if (UserPanel == null)
                            return null;
                        _favoriteButton = UserPanel.Find("Favorite");
                    }
                    return _favoriteButton;
                }
            }
            public static Transform ReportButton
            {
                get
                {
                    if (_reportButton == null)
                    {
                        if (UserPanel == null)
                            return null;
                        _reportButton = UserPanel.Find("Report");
                    }
                    return _reportButton;
                }
            }
            public static Transform UsernameText
            {
                get
                {
                    if (_usernameText == null)
                    {
                        if (UserPanel == null)
                            return null;
                        _usernameText = UserPanel.Find("NameText");
                    }
                    return _usernameText;
                }
            }
            #endregion

            #region User Variables
            private static Transform _userActions;
            #endregion
            #region User Properties
            public static Transform UserActions
            {
                get
                {
                    if (_userActions == null)
                    {
                        if (User == null)
                            return null;
                        _userActions = User.Find("Actions");
                    }
                    return _userActions;
                }
            }
            #endregion

            #region User Actions Variables
            private static Transform _voteKickButton;
            #endregion
            #region User Actions Properties
            public static Transform VoteKickButton
            {
                get
                {
                    if (_voteKickButton == null)
                    {
                        if (UserActions == null)
                            return null;
                        _voteKickButton = UserActions.Find("VoteKick");
                    }
                    return _voteKickButton;
                }
            }
            #endregion

            #region Moderator Variables
            private static Transform _moderatorActions;
            #endregion
            #region Moderator Properties
            public static Transform ModeratorActions
            {
                get
                {
                    if (_moderatorActions == null)
                    {
                        if (Moderator == null)
                            return null;
                        _moderatorActions = Moderator.Find("Actions");
                    }
                    return _moderatorActions;
                }
            }
            #endregion

            #region Moderator Actions Variables
            private static Transform _joinButton;
            #endregion
            #region Moderator Actions Properties
            public static Transform JoinButton
            {
                get
                {
                    if (_joinButton == null)
                    {
                        if (ModeratorActions == null)
                            return null;
                        _joinButton = ModeratorActions.Find("Join!");
                    }
                    return _joinButton;
                }
            }
            #endregion

            #region OnlineFriend Variables
            private static Transform _onlineJoinButton;
            private static Transform _onlineVoteKickButton;
            #endregion
            #region OnlineFriend Properties
            public static Transform OnlineJoinButton
            {
                get
                {
                    if (_onlineJoinButton == null)
                    {
                        if (OnlineFriend == null)
                            return null;
                        _onlineJoinButton = OnlineFriend.Find("JoinButton");
                    }
                    return _onlineJoinButton;
                }
            }
            public static Transform OnlineVoteKickButton
            {
                get
                {
                    if (_onlineVoteKickButton == null)
                    {
                        if (OnlineFriend == null)
                            return null;
                        _onlineVoteKickButton = OnlineFriend.Find("VoteKickButton");
                    }
                    return _onlineVoteKickButton;
                }
            }
            #endregion

            #region OfflineFriend Variables
            private static Transform _offlineJoinButton;
            #endregion
            #region OfflineFriend Properties
            public static Transform OfflineJoinButton
            {
                // Why does this exist?
                get
                {
                    if (_offlineJoinButton == null)
                    {
                        if (OfflineFriend == null)
                            return null;
                        _offlineJoinButton = OfflineFriend.Find("JoinButton");
                    }
                    return _offlineJoinButton;
                }
            }
            #endregion
        }
        public static class InternalSettingsScreen
        {
            #region Settings UI Variables
            private static Transform _volumePanel;
            #endregion
            #region Settings UI Properties
            public static Transform VolumePanel
            {
                get
                {
                    if (_volumePanel == null)
                    {
                        if (SettingsScreen == null)
                            return null;
                        _volumePanel = SettingsScreen.transform.Find("VolumePanel");
                    }
                    return _volumePanel;
                }
            }
            #endregion

            #region VolumePanel Variables
            private static Transform _volumeMaster;
            #endregion
            #region VolumePanel Properties
            public static Transform VolumeMaster
            {
                get
                {
                    if(_volumeMaster == null)
                    {
                        if (VolumePanel == null)
                            return null;
                        _volumeMaster = VolumePanel.Find("VolumeMaster");
                    }
                    return _volumeMaster;
                }
            }
            #endregion
        }
        #endregion
    }
}
