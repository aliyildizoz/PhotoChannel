using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public const string ChannelAdded = "Kanal başarıyla eklendi";
        public const string ChannelDeleted = "Kanal başarıyla silindi";
        public const string ChannelUpdated = "Kanal başarıyla güncellendi";

        public const string SubscribeAdded = "Abonelik başarıyla tamamlandı.";
        public const string SubscribeDeleted = "Abonelik başarıyla iptal edildi.";

        public const string UserNotFound = "Kullanıcı bulunamadı";
        public const string PasswordAndUsernameError = "E-mail veya şifre yanlış.";
        public const string PasswordError = "Şifre hatalı";
        public const string SuccessfulLogin = "Sisteme giriş başarılı";
        public const string UserAlreadyExists = "Böyle bir kullanıcı zaten mevcut.";
        public const string UserRegistered = "Kullanıcı başarıyla kaydedildi";
        public const string AccessTokenCreated = "Access token başarıyla oluşturuldu";

        public const string UserNotAdded = "Kullanıcı Eklenemedi";
        public const string ChannelNameAlreadyExists = "Kanal adı zaten mevcut.";
        public const string ChannelNotFound = "Kanal bulunamadı.";
        public const string PhotoNotFound = "Fotoğraf bulunamadı.";
        public const string CategoryNotFound = "Kategori bulunamadı.";
        public static string PasswordIsNull = "Şifre null veya boş olamaz.";
        public static string UpdatePasswordError = "Girdiğiniz şifre hatalıdır.";
        public static string SearchNotFound = "Böyle bir şey yok.";
    }
}
