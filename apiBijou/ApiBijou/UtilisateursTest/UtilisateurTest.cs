using ApiBijou.Model.Utilisateurs;

namespace UtilisateursTest
{
    public class UtilisateurTest
    {
        //Test de la fonction hashPassword de l'utilisateur Manager
        [Fact]
        public void hashPasswordTest()
        {
            UtilisateursManager utilisateursManager = new UtilisateursManager();
            string pwd = "chat256";
            Assert.Equal(utilisateursManager.HashPassword(pwd), "0da651f3a757364a4a6ce8730990afa46fe8d62e95f26172c47ca2fde814c6f7");
        }

        //Test de la fonction CheckLoginPwd du fake DAO
        [Fact]
        public void CheckLoginPwdTest()
        {
            UtilisateursManager utilisateursManager = new UtilisateursManager();
            string pwd = "chat256";
            Assert.True(utilisateursManager.CheckLoginPwd("root", pwd));
            pwd = "false";
            Assert.False(utilisateursManager.CheckLoginPwd("root", pwd));
        }

        //Test les fonctions IsAdmin et ConnectAsAdmin
        [Fact]
        public void addAdmin()
        {
            UtilisateursManager utilisateursManager = new UtilisateursManager();
            //Bon mdp
            Assert.True(utilisateursManager.ConnectAsAdmin("123456", "root", "chat256"));
            //Muavais mdp
            Assert.False(utilisateursManager.ConnectAsAdmin("123458", "root", "chat257"));
            //On v�rifie que l'admin avec le bon mdp a bien �t� ajout� 
            Assert.True(utilisateursManager.IsAdmin("123456"));
            //On v�rifie que l'utilisateur qui s'est conn�ct� avec un faux mdp n'a pas �t� ajout�
            Assert.False(utilisateursManager.IsAdmin("1234567"));

        }
    }
}