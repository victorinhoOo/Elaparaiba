import { getPanierToken, setPanierToken } from "../js/cookies.js";
import { PanierItemFromJson, displayPanier } from "../js/panier.js";


//Liste des bijoux du panier
var bijouxPanier = [];

async function fetchPanier() {
    const apiUrl = `https://elaparaibatest.fr/Panier/ObtenirPanier?token=${getPanierToken("PanierToken")}`;
    try {
        //Requ�te vers l'Api
        const response = await fetch(apiUrl);
        //Traduction de la requ�te en json
        const panierJson = await response.json();
        //On parcours le �l�ments du json
        for (let i = 0; i < panierJson.length; i++) {
            //Cr�ation d'un panierItem
            const panierItem = PanierItemFromJson(panierJson[i]);
            //Ajout au panier
            bijouxPanier.push(panierItem);
        }

    } catch (error) {
        console.error("Erreur de requ�te:", error);
    }
}

async function updatePanierCount() {
    const apiUrl = `https://elaparaibatest.fr/Panier/ObtenirPanier?token=${getPanierToken("PanierToken")}`;

    // R�cup�re l'�l�ment HTML repr�sentant le nombre d'articles dans le panier
    const panierCountElement = document.getElementById('panierCount'); // L'�l�ment HTML o� afficher le nombre d'articles

    try {
        // Requ�te vers l'API pour obtenir les informations du panier
        const response = await fetch(apiUrl);
        const panierJson = await response.json();

        // Calculer le nombre total d'articles dans le panier
        const nombreArticles = panierJson.reduce((total, panierItem) => total + panierItem.quantite, 0);

        // Mettre � jour l'affichage seulement si le nombre d'articles est sup�rieur � z�ro
        if (nombreArticles > 0) {
            panierCountElement.textContent = nombreArticles.toString();
            panierCountElement.style.display = 'block'; // Afficher l'�l�ment
        } else {
            panierCountElement.style.display = 'none'; // Masquer l'�l�ment si le nombre d'articles est z�ro
        }
    } catch (error) {
        console.error("Erreur lors de la mise � jour du panier:", error);
    }
}

// Fonction pour ajouter le bijou au panier
async function ajouterAuPanier(bijou) {
    //Cr�er un paniertoken si l'utilisateur en a pas
    var panierTokenValue = getPanierToken("PanierToken");
 
    const apiUrl = `https://elaparaibatest.fr/Panier/AjouterAuPanier?token=${panierTokenValue}`; // URL du contr�leur
    try {
        // Requ�te vers l'API avec la m�thode POST
        const response = await fetch(apiUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                NbPhotos: bijou.nbPhotos,
                Id: bijou.idBijou,
                Name: bijou.nomBijou,
                Description: bijou.descriptionBijou,
                Price: bijou.prixBijou,
                Quantity: bijou.stockBijou,
                DatePublication: bijou.datepublication,
                Type: bijou.type,
                DossierPhoto: bijou.dossierPhoto
            }) // Convertit les donn�es du bijou en cha�ne JSON
        });

        if (!response.ok) {
            // La r�ponse n'est pas OK, appeler setPanierToken pour obtenir un nouveau token
            panierTokenValue = await setPanierToken();
            // Mettre � jour l'URL avec le nouveau token
            const newApiUrl = `https://elaparaibatest.fr/Panier/AjouterAuPanier?token=${panierTokenValue}`;
            // Refaire la requ�te avec le nouveau token
            const newResponse = await fetch(newApiUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    NbPhotos: bijou.nbPhotos,
                    Id: bijou.idBijou,
                    Name: bijou.nomBijou,
                    Description: bijou.descriptionBijou,
                    Price: bijou.prixBijou,
                    Quantity: bijou.stockBijou,
                    DatePublication: bijou.datepublication,
                    Type: bijou.type,
                    DossierPhoto: bijou.dossierPhoto
                })
            });

            if (!newResponse.ok) {
                throw new Error('R�ponse r�seau non OK m�me apr�s avoir renouvel� le token.');
            }

            console.log("Requ�te r�ussie apr�s renouvellement du token.");
        } else {
            // La r�ponse est OK
            const responseData = await response.text();
            console.log(responseData);
            updatePanierCount();
        }
    } catch (error) {
        console.error("Erreur de requ�te:", error);
    }
}

// Fonction qui permet de supprimer un bijou du panier
async function supprimerDuPanier(id) {
    var panierTokenValue = getPanierToken("PanierToken");

    const apiUrl = `https://elaparaibatest.fr/Panier/SupprimerDuPanier?token=${panierTokenValue}&id=${id}`;

    try {
        const response = await fetch(apiUrl, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error('R�ponse r�seau non ok');
        }

        // Trouver l'index du bijou dans le panier
        const index = bijouxPanier.findIndex(item => item.id === id);

        // Si l'index est trouv�, supprimer le bijou du panier local
        if (index !== -1) {
            bijouxPanier[index].quantite = bijouxPanier[index].quantite - 1; // D�cr�menter la quantit�
            if (bijouxPanier[index].quantite <= 0) {
                bijouxPanier.splice(index, 1); // Si la quantit� est inf�rieure ou �gale � z�ro, supprimer compl�tement le bijou du panier
            }
        }
        updatePanierCount();
        // Mettre � jour l'affichage du panier
        displayPanier(bijouxPanier);

    } catch (error) {
        console.error("Erreur de requ�te:", error);
    }
}

export { fetchPanier, updatePanierCount, supprimerDuPanier, ajouterAuPanier, bijouxPanier};