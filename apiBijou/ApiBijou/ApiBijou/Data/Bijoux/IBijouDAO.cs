﻿
using ApiBijou.Model.Bijoux;
using System.Diagnostics.Eventing.Reader;

namespace ApiBijou.Data.Bijoux
{
    /// <summary>
    /// Interface des DAO Bijoux
    /// </summary>
    public interface IBijouDAO
    {
        /// <summary>
        /// Récupère un bijou grâce à son id
        /// </summary>
        /// <param name="id">id du bijou</param>
        /// <returns>Bijou renvoyé</returns>
        public Bijou? getById(int id);

        /// <summary>
        /// Récupère tous les bijoux présents dans la base de données
        /// </summary>
        /// <returns>Liste de bijoux</returns>
        public IEnumerable<Bijou> GetAllBijoux();

        /// <summary>
        /// Ajoute un bijou à la base de données
        /// </summary>
        /// <param name="bijou">bijou à ajouter</param>
        /// <returns>vrai si l'ajout a réussi</returns>
        public bool AddBijou(Bijou? bijou);

        /// <summary>
        /// Supprime un bijou
        /// </summary>
        /// <param name="id">id du bijou à supprimer</param>
        /// <returns>vrai si la suppresion a réussi</returns>
        public bool DeleteBijouById(int id);

        /// <summary>
        /// Diminue la quantitié d'un bijou
        /// </summary>
        /// <param name="id">id du bijou</param>
        /// <param name="quantity">quantité à diminuer</param>
        /// <returns>vrai si la diminution du stock a réussi</returns>
        public bool DecreaseStock(int id, int quantity);

    }
}
