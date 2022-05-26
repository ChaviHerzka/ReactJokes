import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useAuthContext } from '../AuthContext';
import useInterval from '../useInterval'
import { Link } from 'react-router-dom';

const Home = () => {

    const [joke, setJoke] = useState({});
    const { user } = useAuthContext();
    const [refresh, setRefresh] = useState(true)
    const [likeEnabled, setLikeEnabled] = useState(true)
    const [dislikeEnabled, setDislikeEnabled] = useState(true)

    useInterval(() => updateCounts(), 2000)

    const updateCounts = async () => {
        const { id: jokeId } = joke;
        if (!jokeId) {
            return;
        }
        const { data } = await axios.get(`/api/joke/getupdatedjoke/${jokeId}`);
        setJoke(formatData(data));
    }


    useEffect(() => {
        const getJoke = async () => {
            const { data } = await axios.get('/api/joke/getjoke');
            setJoke(formatData(data));
            let ulj = data.liked?.find(u => u.jokeId == data.id)
            if (ulj) {
                const { data: oneMinute } = await axios.get(`/api/joke/isbeforeoneminute?date=${ulj.time}`);
                if (!oneMinute) {
                    setLikeEnabled(false)
                    setDislikeEnabled(false)
                }
            }
        };
        getJoke();
    }, [refresh]);

    const formatData = (data) => {
        return {
            id: data.id,
            content: data.content,
            likes: data.liked ? data.liked.filter(liked => liked.liked).length : 0,
            dislikes: data.liked ? data.liked.filter(liked => !liked.liked).length : 0
        }
    }

    const refreshPage = () => {
        setLikeEnabled(true)
        setDislikeEnabled(true)
        setRefresh(!refresh);
    };


    const buttonClick = async like => {
        if (likeEnabled && dislikeEnabled) {
            await axios.post(`/api/joke/addlike`, { userId: user.id, jokeId: joke.id, liked: like });
        }
        else {
            const { data } = await axios.post(`/api/joke/updatelike`, { userId: user.id, jokeId: joke.id, liked: like })
        };
        updateCounts()


        if (like) {
            setLikeEnabled(false)
            setDislikeEnabled(true)
        }
        else {
            setDislikeEnabled(false)
            setLikeEnabled(true)
        }

    };


    return (
        <div className='row'>
            <div className='col-md-6 offset-md-3 card card-body bg-light'>
                <div>
                    <h4>{joke.content}</h4>
                </div>
                {joke.content && !user ? <Link to='/login'>Login to your account to like/dislike a joke</Link> :
                    <div>
                        <button className='btn btn-info' disabled={!likeEnabled} onClick={() => buttonClick(true)}>Like</button>
                        <button className='btn btn-warning' disabled={!dislikeEnabled} onClick={() => buttonClick(false)}>DisLike</button>
                    </div>}
                <br />
                <h4>Likes: {joke.likes} </h4>
                <h4>Dislikes: {joke.dislikes} </h4>
                <h4>
                    <button className='btn btn-link' onClick={refreshPage}>Refresh</button>
                </h4>
            </div>
        </div>
    )
}
export default Home;